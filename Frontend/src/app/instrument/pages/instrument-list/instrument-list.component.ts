import { Component, computed, effect, inject, signal, untracked, viewChild, WritableSignal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators, FormControlStatus } from '@angular/forms';
import { InstrumentFilterQuery, InstrumentResponse, InstrumentType, UpdateInstrumentCommand } from '../../interfaces';
import { FilterLayoutComponent } from '../../../shared/components/filter-layout/filter-layout.component';
import { IntrumentService } from '../../services/intrument.service';
import { DtoWithId, PagedResult, SelectItem } from '../../../shared/interfaces';
import { ToastrService } from 'ngx-toastr';
import { ErrorUtilitiesClass } from '../../../shared/interfaces/error-utilities.class';
import { DialogModalComponent } from "../../../shared/components/dialog-modal/dialog-modal.component";
import { TableComponent } from '../../../shared/components/table/table.component';
import { PagerComponent } from "../../../shared/components/pager/pager.component";
import { ItemsPerPageComponent } from "../../../shared/components/items-per-page/items-per-page.component";
import { CreateInstrumentCommand } from '../../interfaces/create-instrument-command.interface';
import { ValidatorsService } from '../../../shared/services/validator.service';
import { FieldErrorDirective } from "../../../shared/directives/field-error-directive";
import { toSignal } from '@angular/core/rxjs-interop';
import { map, startWith } from 'rxjs';
import { InputSearchSelectorComponent } from "../../../shared/components/input-search-selector/input-search-selector.component";
import { MusicianResponse, SearchMusicianByFilterQuery } from '../../../musician/interfaces';
import { environment } from '../../../../environments/environment.development';

@Component({
  selector: 'app-instrument-list',
  imports: [ReactiveFormsModule, FilterLayoutComponent, TableComponent, DialogModalComponent, PagerComponent, ItemsPerPageComponent, FieldErrorDirective, InputSearchSelectorComponent],
  templateUrl: './instrument-list.component.html',
  styleUrl: './instrument-list.component.css',
})
export default class InstrumentListComponent {

  private readonly _validatorsService = inject(ValidatorsService);

  private _fb = inject(FormBuilder);
  instrumentFilterForm = this._fb.group({
    name: ['', Validators.pattern(this._validatorsService.musicalInstrumentNamePattern)],
    country: ['', Validators.pattern(this._validatorsService.countryNamePattern)],
    type: this._fb.control<number | null>(null),
  });

  instrumentDialogModalForm = this._fb.group({
    id: this._fb.control<number | null>(null),
    name: ['',[Validators.required,  Validators.pattern(this._validatorsService.musicalInstrumentNamePattern)]],
    country: ['',[Validators.required, Validators.pattern(this._validatorsService.countryNamePattern)]],
    type: this._fb.control<InstrumentType | null>(null, Validators.required),
    description: [''],
    musicianId: this._fb.control<number | null>(null, Validators.required),
  });

  //#region private fields
  private readonly _instrumentService = inject(IntrumentService);
  private readonly _toastService = inject(ToastrService);
  private readonly dialogModal = viewChild<DialogModalComponent>('dialogModalComponent');
  private readonly promptModal = viewChild<DialogModalComponent>('promptModalComponent');
  private readonly _instrumentIdToDelete = signal<number | null>(null);
  private _instrumentFilterQuery: WritableSignal<InstrumentFilterQuery> = signal({
    page: 1,
    pageSize: 10,
    requestCount: true,
  });

  private readonly selectedIdsSelectedForDelete = signal<number[]>([]);
  public isDeleteSingleMode = signal(false);
  //#endregion

  //#region public fields
  public promptDeleteModalBodyText = signal<string>('');
  public deleteAllButtonDisabled = computed(() => !this.selectedIdsSelectedForDelete()?.length);
  public initialDto = signal<SelectItem | null>(null);
  public callBackShowFieldValueFn = (item: DtoWithId) => ({
    id: item.id,
    text: `${(item as MusicianResponse).firstName} ${(item as MusicianResponse).lastName}`,
  });

  public searchPostUri = `${environment.API_BASE_URL}/api/musician/search`;
  public callBackGetFilterFn = (query: string) => ({
    page: 1,
    pageSize: 10,
    firstName: query,
  }) as SearchMusicianByFilterQuery;

  public isLoading = signal(false);
  public page = signal(1);
  public itemsPerPage = signal(20);
  public instrumentDialogModalTitle = signal('Add Instrument');
  public promptDeleteModalTitle = signal<string>('Delete Instrument');
  public readonly disableDialogModalOkBtn = toSignal(
    this.instrumentDialogModalForm.statusChanges.pipe(
    map(status => status === 'INVALID'),
    startWith(this.instrumentDialogModalForm.invalid)
  ));

  public instrumentFilterEffect = effect(() => {
    const filterQuery = this._instrumentFilterQuery();
    this.doSearchByFilter(filterQuery);
  });

  public instrumentList = signal<PagedResult<InstrumentResponse>>({
    currentpage: 1,
    pageSize: 20,
    itemCount: 0,
    hasNextPage: false,
    displayFrom: 0,
    displayTo: 0,
    results: [],
  });
  public pageCount = computed(() => this.instrumentList().pageCount ?? 1);

  public get instrumentTypeOptions() {
    return Object.keys(InstrumentType)
      .filter(key => isNaN(Number(key)))
      .map(
        key => ({
          label: key,
          value: Number(InstrumentType[key as keyof typeof InstrumentType])
        })
      );
  }
  //#endregion

  //#region public methods

  public setMusicianId(musicianId: number | null) {
    this.instrumentDialogModalForm.get('musicianId')?.setValue(musicianId);
  }

  public getInstrumentTypeLabel(type: number){
    return InstrumentType[type];
  }

  onFilterSearch() {
    if (this.instrumentFilterForm.invalid) {
      this._toastService.error('Invalid filter values.');
    }
    this._instrumentFilterQuery.set(this.getInstrumentsByFilter());
  }

  onFilterClear() {
    if (this.instrumentFilterForm.dirty) {
      this.instrumentFilterForm.reset();
      this._instrumentFilterQuery.set(this.getInstrumentsByFilter());
    }
  }

  // Table methods
  onDeleteAllButtonClicked() {
    if(this.selectedIdsSelectedForDelete()?.length){
      this.promptDeleteModalBodyText.set('Are you sure you want to delete the selected instruments?');
      this.isDeleteSingleMode.set(false);
      this.promptModal()?.showModal();
    }

  }

  tableItemsPerPageChanged(itemsPerPage: number) {
    this.itemsPerPage.set(itemsPerPage);
    this._instrumentFilterQuery.set(this.getInstrumentsByFilter());
  }

  tablePageChanged(page: number) {
    this.page.set(page);
    this._instrumentFilterQuery.set(this.getInstrumentsByFilter());
  }

  public updateSelectedInstrumentForDelete(instrumentId: number, event: Event){
    const target = event.target as HTMLInputElement;
    if(instrumentId && target){
      if(target.checked){
        if(this.selectedIdsSelectedForDelete().find(i => i == instrumentId) === undefined){
            this.selectedIdsSelectedForDelete.update(current => [...current, instrumentId]);
            return;
        }
      }
      this.selectedIdsSelectedForDelete.update(current => current.filter(i => i != instrumentId));
    }
  }

  // Modal methods
  showModalOnCreateMode() {
    this.instrumentDialogModalTitle.set('Add Instrument');
    this.instrumentDialogModalForm.reset();
    this.initialDto.set(null);
    this.dialogModal()?.showModal();
  }

  showModalOnUpdateMode(instrumentId: number) {
    const instrument = this.instrumentList().results.find(i => i.id === instrumentId);
    if (!instrument) {
      this._toastService.error('Instrument not found');
      return;
    }
    this.instrumentDialogModalTitle.set('Update Instrument');
    this.initialDto.set({
      id: instrument.musicianId,
      text: instrument.musicianName
    });
    this.instrumentDialogModalForm.patchValue(instrument);
    this.dialogModal()?.showModal();
  }


  onAcceptModalButtonClicked() {
    if (this.isLoading()) {
      this._toastService.info('A request is already in progress. Please wait...');
      return;
    }

    this.isLoading.set(true);


    const { id, name, country, description, musicianId, type } = this.instrumentDialogModalForm.value;
    const instrument = {
      name,
      country,
      description,
      musicianId: Number(musicianId),
      type: Number(type)
    };

    let message = 'Instrument updated successfully';
    let doTask = this._instrumentService.updateInstrument({ ...instrument, id: Number(id) } as UpdateInstrumentCommand)

    if (!id) {
      message = 'Instrument created successfully';
      doTask = this._instrumentService.createInstrument(instrument as CreateInstrumentCommand)
    }

    doTask.subscribe({
      next: () => {
        this.isLoading.set(false);
        this._instrumentFilterQuery.set(this.getInstrumentsByFilter());
        this._toastService.success(message, 'Success');
        this.dialogModal()?.closeModal();
      },
      error: (err) => {
        this.isLoading.set(false);
        this._toastService.error(ErrorUtilitiesClass.getErrorMessage(err), 'Error');
      }
    })
  }

  onCancelModalButtonClicked() {
    this.instrumentDialogModalForm.reset();
    this.dialogModal()?.closeModal();
  }

  showConfirmDeleteInstrument(instrumentId: number) {
    if (!instrumentId) {
      this._toastService.error('Instrument not found');
      return;
    }
    const instrument = this.instrumentList().results.find(i => i.id === instrumentId);
    if(!instrument){
      this._toastService.error('Instrument not found');
      return;
    }

    this.promptDeleteModalTitle.set('Delete Instrument');
    this.promptDeleteModalBodyText.set('Are you sure you want to delete the instrument: ${instrument.name} ?');
    this.isDeleteSingleMode.set(false);
    this._instrumentIdToDelete.set(instrumentId);
    this.promptModal()?.showModal();
  }

  onDeletePromptButtonClicked() {
    if(this.isDeleteSingleMode()){
      this.deleteSingleInstrument();
      return;
    }

    this.deleteManyInstruments();
  }

  public deleteSingleInstrument(){
    const instrumentId = this._instrumentIdToDelete();
    if (!instrumentId) {
      this._toastService.error('Instrument not found');
      return;
    }

    this._instrumentService.deleteInstrument(instrumentId)
      .subscribe({
        next: () => {
          this._instrumentIdToDelete.set(null);
          this.promptDeleteModalTitle.set('Delete Instrument');
          this._toastService.success('Instrument deleted successfully', 'Success');
          this._instrumentFilterQuery.set(this.getInstrumentsByFilter());
          this.promptModal()?.closeModal();
        },
        error: (err) => {
          this._toastService.error(ErrorUtilitiesClass.getErrorMessage(err), 'Error');
        }
      })
  }
  public deleteManyInstruments(){
    if(this.selectedIdsSelectedForDelete().length == 0){
      this._toastService.error('Please select at least one instrument');
      return;
    }
    this._instrumentService.deteleManyInstruments(this.selectedIdsSelectedForDelete())
    .subscribe({
      next: () => {
        this.selectedIdsSelectedForDelete.set([]);
        this._toastService.success('Instruments deleted successfully', 'Success');
        this._instrumentFilterQuery.set(this.getInstrumentsByFilter());
        this.promptModal()?.closeModal();
      },
      error: (err) => {
        this._toastService.error(ErrorUtilitiesClass.getErrorMessage(err), 'Error');
      }
    })
  }

  public onCancelPromptButtonClicked() {
    this._instrumentIdToDelete.set(null);
    this.promptDeleteModalTitle.set('Delete Instrument');
    this.promptModal()?.closeModal();
  }

  //Validation Methods

  public isInvalidField(controlName: string, isFilterFormGroup: boolean = false): boolean{
    const formGroup = isFilterFormGroup ? this.instrumentFilterForm : this.instrumentDialogModalForm;
    return this._validatorsService.isInvalidField(formGroup, controlName);
  }

  public getErrorMessage(constrolName: string, isFilterFormGroup: boolean = false): string{
    const formGroup = isFilterFormGroup ? this.instrumentFilterForm : this.instrumentDialogModalForm;
    return this._validatorsService.getFieldError(formGroup, constrolName);
  }

  //#endregion

  //#region private methods

  private getInstrumentsByFilter(): InstrumentFilterQuery {
    const filterValue = this.instrumentFilterForm.value;

    const cleaned = Object.entries(filterValue)
      .filter(([_, value]) =>
        value != null &&
        value !== '' &&
        value !== undefined &&
        value !== 0
      )
      .reduce((acc, [key, value]) => {
        acc[key] = isNaN(Number(value)) ? (value as string) : Number(value);
        return acc;
      }, {} as any)

      return {
      ...cleaned,
      page: this.page(),
      pageSize: this.itemsPerPage(),
      requestCount: true
    };
  }

  private doSearchByFilter(instrumentFilterQuery: InstrumentFilterQuery) {
    if (untracked(() => this.isLoading())) {
      this._toastService.info('A request is already in progress. Please wait...');
      return;
    }
    this.isLoading.set(true);
    this._instrumentService
      .searchInstrumentsByFilter(instrumentFilterQuery)
      .subscribe({
        next: (response) => {
          this.instrumentList.set(response);
          this.isLoading.set(false);
        },
        error: (err) => {
          this._toastService.error(ErrorUtilitiesClass.getErrorMessage(err), 'Error');
          this.isLoading.set(false);
        }
      });
  }

  //#endregion

}
