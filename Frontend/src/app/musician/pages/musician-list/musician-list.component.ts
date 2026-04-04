import { Component, computed, effect, inject, signal, viewChild, WritableSignal, untracked } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { toSignal } from '@angular/core/rxjs-interop';
import { CurrencyPipe } from '@angular/common';

import { ToastrService } from 'ngx-toastr';
import { map, startWith, tap } from 'rxjs';

import { FilterLayoutComponent } from "../../../shared/components/filter-layout/filter-layout.component";
import { TableComponent } from "../../../shared/components/table/table.component";
import { CreateMusicianCommand, MusicianResponse, SearchMusicianByFilterQuery, UpdateMusicianCommand } from '../../interfaces';
import { MusicianService } from '../../services/musician.service';
import { ErrorUtilitiesClass } from '../../../shared/interfaces/error-utilities.class';
import { FieldErrorDirective } from '../../../shared/directives/field-error-directive';
import { ValidatorsService } from '../../../shared/services/validator.service';
import { PagedResult } from './../../../shared/interfaces/paged-result.interface';
import { DialogModalComponent } from "../../../shared/components/dialog-modal/dialog-modal.component";
import { ItemsPerPageComponent } from "../../../shared/components/items-per-page/items-per-page.component";
import { PagerComponent } from "../../../shared/components/pager/pager.component";
import { InstrumentService } from '../../../instrument/services/instrument.service';
import { DtoWithId, SelectItem } from '../../../shared/interfaces';
import { InstrumentResponse } from '../../../instrument/interfaces';
import { environment } from '../../../../environments/environment.development';
import { InputSearchSelectorComponent } from "../../../shared/components/input-search-selector/input-search-selector.component";

@Component({
  selector: 'app-musician-list',
  imports: [FilterLayoutComponent, TableComponent, ReactiveFormsModule, FieldErrorDirective, CurrencyPipe, DialogModalComponent, ItemsPerPageComponent, PagerComponent, InputSearchSelectorComponent],
  templateUrl: './musician-list.component.html',
  styleUrl: './musician-list.component.css',
})
export default class MusicianList  {

  //#region Private Properties

  private readonly _musicianService = inject(MusicianService);
  private readonly _instrumentService = inject(InstrumentService);
  private readonly _fb = inject(FormBuilder);
  private readonly _validatorService = inject(ValidatorsService);
  private readonly _toastService = inject(ToastrService);
  private readonly musicianToDeleteId: WritableSignal<number | null> = signal(null);

  //#endregion

  //#region Public Properties

  public initialDto = signal<SelectItem | null>(null);
    public callBackShowFieldValueFn = (item: DtoWithId) => ({
      id: item.id,
      text: (item as InstrumentResponse).name,
    } as SelectItem);

    public searchPostUri = `${environment.API_BASE_URL}/api/instrument/search`;
    public callBackGetFilterFn = (query: string) => ({
      page: 1,
      pageSize: 10,
      name: query,
    }) as SearchMusicianByFilterQuery;

  public dialogModalComponent = viewChild<DialogModalComponent>('dialogModalComponent');
  public promptModalComponent = viewChild<DialogModalComponent>('promptModalComponent');
  public isLoading: WritableSignal<boolean> = signal(false);
  musicianPagedResult: WritableSignal<PagedResult<MusicianResponse>> = signal({
    itemCount: 0,
    pageSize: 20,
    pageCount: 1,
    currentpage: 1,
    hasNextPage: false,
    displayFrom: 0,
    displayTo: 0,
    totalItemCount: 0,
    results: [],
  });
  public pageQuery = signal(this.musicianPagedResult().currentpage);
  public pageSizeQuery = signal(this.musicianPagedResult().pageSize);
  public readonly pageCount = computed(() => this.musicianPagedResult().pageCount || 1);
  public musicians = computed(() => this.musicianPagedResult().results);



  musicianFilterformGroup = this._fb.group({
    firstName: ['', [Validators.maxLength(25)]],
    lastName: ['', [Validators.maxLength(50)]],
    fromAge: [0, [Validators.min(0), Validators.max(80)]],
    toAge: [0, [Validators.min(0), Validators.max(80)]],
    fromExperience: [0, [Validators.min(0), Validators.max(50)]],
    toExperience: [0, [Validators.min(0), Validators.max(80)]],
    fromBasicSalary: [0, [Validators.min(0), Validators.max(100000)]],
    toBasicSalary: [0, [Validators.min(0), Validators.max(100000)]],
    instrumentId: [0, [Validators.min(0)]],
  });

  musicianDialogModalForm = this._fb.group({
    id: this._fb.control<number | null>(null),
    firstName: ['', [Validators.required, Validators.maxLength(25)]],
    lastName: ['', [Validators.required, Validators.maxLength(50)]],
    age: [0, [Validators.required, Validators.min(0), Validators.max(80)]],
    experience: [0, [Validators.required, Validators.min(0), Validators.max(50)]],
    basicSalary: [0, [Validators.required, Validators.min(0), Validators.max(100000)]]
  });

  public readonly disableDialogModalOkBtn = toSignal(
    this.musicianDialogModalForm.statusChanges
    .pipe(
      map((status) => status === 'INVALID'),
      startWith(this.musicianDialogModalForm.invalid)
    )
  );

  searchMusicianQuery: WritableSignal<SearchMusicianByFilterQuery> = signal<SearchMusicianByFilterQuery>(
    {
      page: 1,
      pageSize: 20,
      requestCount: true
    }
  );

  searchFilterQueryEffect = effect(() => {
    const musicianQuery = this.searchMusicianQuery();
    this.doMusicianSearchByQuery(musicianQuery);
  })

  public musicianSelectedForDelete: WritableSignal<number[]> = signal([]); // musicianSelectedForDelete
  public musicianSelectedForDeleteEmpty = computed(() => !this.musicianSelectedForDelete()?.length);
  public musicianSingleDeleteMode = signal(false);
  public promptModalBodyText = signal('Are you sure to delete?');

  //#endregion




  public get musicianDialogModalTitle() {
    return !this.musicianFilterformGroup.get('id')?.value ? 'Add Musician' : 'Update Musician';
  }


  //#region Public Methods
  onSearch() {
    this.searchMusicianQuery.set(this.searchMusicianByFilterQuery);
  }

  onReset() {
    if (this.musicianFilterformGroup.dirty) {
      this.musicianFilterformGroup.reset();
      this.searchMusicianQuery.set(this.searchMusicianByFilterQuery);
    }
  }

  public setFilterInstrumentId(id: number | null) {
    this.musicianFilterformGroup.get('instrumentId')?.setValue(id);
  }

  public isInvalidField(controlName: string, isFilterFormGroup: boolean = false): boolean {
    const formGroup = isFilterFormGroup ? this.musicianFilterformGroup : this.musicianDialogModalForm;
    return this._validatorService.isInvalidField(formGroup, controlName);
  }

  public getErrorMessage(controlName: string, isFilterFormGroup: boolean = false, fieldNameToShow?: string): string | null {
    const formGroup = isFilterFormGroup ? this.musicianFilterformGroup : this.musicianDialogModalForm;
    return this._validatorService.getFieldError(formGroup, controlName, fieldNameToShow);
  }


  showModalOnCreateMode() {
    this.musicianDialogModalForm.reset();
    this.dialogModalComponent()?.showModal();
  }

  onAcceptPromptButtonClicked() {

    if (this.musicianSingleDeleteMode()) {
      this.deleteSingleMusician();
      return;
    }
    this.deleteManyMusician();
  }

  public deleteManyMusician() {
    return this._musicianService.deleteManyMusician(this.musicianSelectedForDelete())
      .subscribe({
        next: () => {
          this._toastService.success('Musician deleted successfully', 'Success');
          this.musicianSelectedForDelete.set([]);
          this.searchMusicianQuery.set(this.searchMusicianByFilterQuery);
          this.promptModalComponent()?.closeModal();
        },
        error: (error: any) => {
          this._toastService.error(ErrorUtilitiesClass.getErrorMessage(error), 'Error');
        }
      })
  }

  public deleteSingleMusician() {
    if(this.musicianToDeleteId() === null) {
      this._toastService.error('Invalid Musician Id');
      return;
    }

    this._musicianService.deleteMusician(this.musicianToDeleteId() ?? 0)
      .pipe(
        tap(() => this.promptModalComponent()?.closeModal())
      ).subscribe({
        next: () => {
          if(this.musicianSelectedForDelete()?.includes(this.musicianToDeleteId() ?? 0)) {
            this.musicianSelectedForDelete.update(current => current.filter(id => id !== this.musicianToDeleteId()));
          }
          this._toastService.success('Musician deleted successfully', 'Success');
          this.searchMusicianQuery.set(this.searchMusicianByFilterQuery);
          this.musicianToDeleteId.set(null);
          this.promptModalComponent()?.closeModal();
        },
        error: (error: any) => {
          this._toastService.error(ErrorUtilitiesClass.getErrorMessage(error), 'Error');
        }
      });
  }

  onCancelPromptButtonClicked() {
    this.musicianToDeleteId.set(null);
    this.promptModalComponent()?.closeModal();
  }

  onAcceptModalButtonClicked() {
    if (this.isLoading()) {
      this._toastService.info('A request is already in progress. Please wait...');
      return;
    }
    this.isLoading.set(true);

    const musician = this.musicianDialogModalForm.value;
    let message = 'Musician updated successfully';
    let doTask = this.doUpdateMusician(musician as UpdateMusicianCommand);

    if (!this.musicianDialogModalForm.get('id')?.value) {
      doTask = this.doCreateMusician(musician as CreateMusicianCommand);
      message = 'Musician added successfully';
    }

    doTask.subscribe({
      next: () => {
        this._toastService.success(message, 'Success');
        this.dialogModalComponent()?.closeModal();
        this.searchMusicianQuery.set(this.searchMusicianByFilterQuery);
        this.isLoading.set(false);
      },
      error: (error: any) => {
        this._toastService.error(ErrorUtilitiesClass.getErrorMessage(error), 'Error');
        this.isLoading.set(false);
      }
    });
    this.searchMusicianQuery.set(this.searchMusicianByFilterQuery);
  }

  onDeleteAllButtonClicked() {
    if (!this.musicianSelectedForDeleteEmpty()) {
      this.promptModalBodyText.set('Are you sure to delete the selected musicians?');
      this.musicianSingleDeleteMode.set(false);
      this.promptModalComponent()?.showModal();
    }
  }

  public updateMusicianSelectedForDelete(musicianId: number, event: Event) {
    const target = event.target as HTMLInputElement;
    if (musicianId && target) {
      if (target.checked) {
        this.musicianSelectedForDelete.update(current => [...current, musicianId]);
        return;
      }
      this.musicianSelectedForDelete.update(current => current.filter(mid => mid !== musicianId));
    }
  }


  showDeletePromptModal(musicianId: number) {
    if (!musicianId) {
      this._toastService.error('Invalid Musician Id');
      return;
    }
    const musician = this.musicianPagedResult().results.find(m => m.id === musicianId);
    if (!musician) {
      this._toastService.error('Musician not found');
      return;
    }
    this.musicianToDeleteId.set(musician.id);
    this.musicianSingleDeleteMode.set(true);
    this.promptModalBodyText.set(`Are you sure to delete the musician: ${musician.firstName + ' ' + musician.lastName} ?`);
    this.promptModalComponent()?.showModal();
  }

  showModalOnUpdateMode(musicianId: number) {
    const musician = this.musicianPagedResult().results.find(m => m.id === musicianId);
    if (!musician) {
      this._toastService.error('Musician not found');
      return;
    }
    this.musicianDialogModalForm.patchValue(musician);
    this.dialogModalComponent()?.showModal();
  }

  tableItemsPerPageChanged(pageSize: number) {
    this.pageSizeQuery.set(pageSize);
    this.searchMusicianQuery.set(this.searchMusicianByFilterQuery);
  }

  tablePageChanged(page: number) {
    this.pageQuery.set(page);
    this.searchMusicianQuery.set(this.searchMusicianByFilterQuery);
  }

  //#endregion

  //#region Private Methods
  get searchMusicianByFilterQuery(): SearchMusicianByFilterQuery {
    const formValue = this.musicianFilterformGroup.value;
    const cleaned = Object.entries(formValue)
      .filter(([_, value]) =>
        value != null &&
        value !== '' &&
        value !== undefined &&
        value !== 0)
      .reduce((acc, [key, value]) => {
        acc[key as any] = isNaN(value as number) ? (value as string) as any : (value as number) as any;
        return acc;
      }, {} as any);

    const musicianQuery = {
      ...cleaned,
      page: this.pageQuery(),
      pageSize: this.pageSizeQuery(),
      requestCount: true
    } as SearchMusicianByFilterQuery;

    return musicianQuery;
  }

  private doMusicianSearchByQuery(musicianQuery: SearchMusicianByFilterQuery) {
    if (untracked(() => this.isLoading())) {
      this._toastService.info('A request is already in progress. Please wait...');
      return;
    };

    this.isLoading.set(true);
    this._musicianService.searchMusicianByQuery(musicianQuery)
      .subscribe({
        next: (response) => {
          this.musicianPagedResult.set(response);
          this.isLoading.set(false);
        },
        error: (err) => {
          this._toastService.error(ErrorUtilitiesClass.getErrorMessage(err), 'Error');
          this.isLoading.set(false);
        }
      })
  }

  private doCreateMusician(musician: CreateMusicianCommand) {
    return this._musicianService.createMusician(musician);
  }

  private doUpdateMusician(musician: UpdateMusicianCommand) {
    return this._musicianService.updateMusician(musician);
  }

  //#endregion
}
