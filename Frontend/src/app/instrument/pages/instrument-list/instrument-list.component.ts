import { Component, computed, effect, inject, signal, untracked, viewChild, WritableSignal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { InstrumentFilterQuery, InstrumentResponse, InstrumentType, UpdateInstrumentCommand } from '../../interfaces';
import { FilterLayoutComponent } from '../../../shared/components/filter-layout/filter-layout.component';
import { IntrumentService } from '../../services/intrument.service';
import { PagedResult } from '../../../shared/interfaces';
import { ToastrService } from 'ngx-toastr';
import { ErrorUtilitiesClass } from '../../../shared/interfaces/error-utilities.class';
import { DialogModalComponent } from "../../../shared/components/dialog-modal/dialog-modal.component";
import { TableComponent } from '../../../shared/components/table/table.component';
import { PagerComponent } from "../../../shared/components/pager/pager.component";
import { ItemsPerPageComponent } from "../../../shared/components/items-per-page/items-per-page.component";
import { CreateInstrumentCommand } from '../../interfaces/create-instrument-command.interface';
import { ValidatorsService } from '../../../shared/services/validator.service';
import { FieldErrorDirective } from "../../../shared/directives/field-error-directive";

@Component({
  selector: 'app-instrument-list',
  imports: [ReactiveFormsModule, FilterLayoutComponent, TableComponent, DialogModalComponent, PagerComponent, ItemsPerPageComponent, FieldErrorDirective],
  templateUrl: './instrument-list.component.html',
  styleUrl: './instrument-list.component.css',
})
export default class InstrumentListComponent {

  private readonly _validatorsService = inject(ValidatorsService);

  private _fb = inject(FormBuilder);
  instrumentFilterForm = this._fb.group({
    name: ['', Validators.pattern(this._validatorsService.musicalInstrumentNamePattern)],
    country: ['', Validators.pattern(this._validatorsService.countryNamePattern)],
    type: this._fb.control<InstrumentType | null>(null),
  });

  instrumentDialogModalForm = this._fb.group({
    id: this._fb.control<number | null>(null),
    name: ['',Validators.pattern(this._validatorsService.musicalInstrumentNamePattern)],
    country: ['', Validators.pattern(this._validatorsService.countryNamePattern)],
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
  });
  //#endregion

  //#region public fields
  public isLoading = signal(false);
  public page = signal(1);
  public itemsPerPage = signal(20);
  public instrumentDialogModalTitle = signal('Add Instrument');
  public promptDeleteModalTitle = signal<string>('Delete Instrument');

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
          value: InstrumentType[key as keyof typeof InstrumentType]
        })
      );
  }
  //#endregion

  //#region public methods

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

  }

  tableItemsPerPageChanged(itemsPerPage: number) {
    this.itemsPerPage.set(itemsPerPage);
    this._instrumentFilterQuery.set(this.getInstrumentsByFilter());
  }

  tablePageChanged(page: number) {
    this.page.set(page);
    this._instrumentFilterQuery.set(this.getInstrumentsByFilter());
  }

  // Modal methods

  showModalOnCreateMode() {
    this.instrumentDialogModalTitle.set('Add Instrument');
    this.instrumentDialogModalForm.reset();
    this.dialogModal()?.showModal();
  }

  showModalOnUpdateMode(instrumentId: number) {
    const instrument = this.instrumentList().results.find(i => i.id === instrumentId);
    if (!instrument) {
      this._toastService.error('Instrument not found');
      return;
    }
    this.instrumentDialogModalTitle.set('Update Instrument');
    this.instrumentDialogModalForm.patchValue(instrument);
    this.dialogModal()?.showModal();
  }


  onAcceptModalButtonClicked() {
    if (this.isLoading()) {
      this._toastService.info('A request is already in progress. Please wait...');
      return;
    }

    this.isLoading.set(true);

    const instrument = this.instrumentDialogModalForm.value;
    let message = 'Instrument updated successfully';
    let doTask = this._instrumentService.updateInstrument(instrument as UpdateInstrumentCommand)

    if (instrument.id == null) {
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

  showConfirmDeleteInstrument(instrumentId: number, name: string) {
    if (!instrumentId) {
      this._toastService.error('Instrument not found');
      return;
    }

    this._instrumentIdToDelete.set(instrumentId);
    this.promptDeleteModalTitle.set(`Delete Instrument ${name} ?`);
    this.promptModal()?.showModal();
  }

  onDeletePromptButtonClicked() {
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
        acc[key] = value as any;
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
