import { Component, computed, effect, inject, signal, untracked, viewChild } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { PaymentDetailService } from '../../services/payment-details.service';
import { ValidatorsService } from '../../../shared/services/validator.service';
import { ToastrService } from 'ngx-toastr';
import { SearchPaymentDetailsByFilterQuery } from '../../interfaces/search-payment-by-filter-query.interface';
import { ErrorUtilitiesClass } from '../../../shared/interfaces/error-utilities.class';
import { PaymentDetailResponse } from '../../interfaces/payment-detail-response.interface';
import { PagedResult } from '../../../shared/interfaces';
import { map, startWith } from 'rxjs';
import { toSignal } from '@angular/core/rxjs-interop';
import { DialogModalComponent } from '../../../shared/components/dialog-modal/dialog-modal.component';
import { CreatePaymentDetailCommand, UpdatePaymentDetailCommand } from '../../interfaces';
import { FieldErrorDirective } from '../../../shared/directives/field-error-directive';
import { TableComponent } from "../../../shared/components/table/table.component";
import { ItemsPerPageComponent } from "../../../shared/components/items-per-page/items-per-page.component";
import { PagerComponent } from "../../../shared/components/pager/pager.component";
import { DatePipe, DecimalPipe } from '@angular/common';
import { FilterLayoutComponent } from '../../../shared/components/filter-layout/filter-layout.component';
import { RangePlusService } from '../../../range-plus/services/range-plus.service';
import { RangePlusResponse } from '../../../range-plus/interfaces/range-plus-response.interface';

@Component({
  selector: 'app-payment-list',
  imports: [
    ReactiveFormsModule,
    FilterLayoutComponent,
    FieldErrorDirective,
    DialogModalComponent,
    TableComponent,
    ItemsPerPageComponent,
    PagerComponent,
    DatePipe,
    DecimalPipe
  ],
  templateUrl: './payment-list.component.html',
  styleUrl: './payment-list.component.css',
})
export default class PaymentListComponent {
  private readonly _paymentDetailService = inject(PaymentDetailService);
  private readonly _rangePlusService = inject(RangePlusService);
  private readonly _validatorService = inject(ValidatorsService);
  private readonly _toastService = inject(ToastrService);
  private readonly _fb = inject(FormBuilder);
  //#region Private Fields
  private readonly _paymentDetailsFilterQuery = signal<SearchPaymentDetailsByFilterQuery>({
    page: 1,
    pageSize: 20,
    requestCount: true,
  });

  pageCount = computed(() => this.paymentDetailPagedResult().pageCount || 1);
  rangePlusList = signal<RangePlusResponse[]>([]);
  paymentIdsSelectedForDelete = signal<number[]>([]);
  paymentIdsSelectedForDeleteEmpty = computed(() => !this.paymentIdsSelectedForDelete()?.length);

  paymentDetailFilterQueryEffect = effect(() => {
    const activityFilter = this._paymentDetailsFilterQuery();
    this.doSearchByFilter(activityFilter);
  });

  rangePlusListEffect = effect(() => this._doGetAllRangePlus());

  private paymentDetailIdToDelete = signal<number | null>(null);
  private isPromptDeleteSingleMode = signal(false);

  //#endregion

  //#region public fields
  public isLoading = signal(false);

  public paymentDetailPagedResult = signal<PagedResult<PaymentDetailResponse>>({
    currentpage: 1,
    pageSize: 20,
    itemCount: 20,
    hasNextPage: false,
    displayFrom: 1,
    displayTo: 1,
    results: [],
  });


  public payments = computed(() => this.paymentDetailPagedResult().results);

  public page = signal(1);
  public itemsPerPage = signal(20);

  public filterForm = this._fb.group({
    fromSalary: this._fb.control<number | null>(null),
    toSalary: this._fb.control<number | null>(null),
    fromPaymentDate: this._fb.control<Date | null>(null),
    toPaymentDate: this._fb.control<Date | null>(null),
  });

  public dialogModalForm = this._fb.group({
    id: this._fb.control<number | null>(null),
    paymentDate: this._fb.control<string | null>(null),
    salary: [0, [Validators.required, Validators.min(0)]],
    basicSalary: [0, [Validators.required, Validators.min(0)]],
    musicianId: [0, [Validators.required, Validators.min(0)]],
    rangePlusId: this._fb.control<number | null>(null, [Validators.required])
  });

  public disableDialogModalOkBtn = toSignal(this.dialogModalForm.statusChanges
    .pipe(
      map(status => status === 'INVALID'),
      startWith(this.dialogModalForm.invalid)
    )
  );

  public dialogModal = viewChild<DialogModalComponent>('dialogModal');
  public promptModal = viewChild<DialogModalComponent>('promptModal');
  public dialogModalTitle = signal('');
  public deletePromptBodyText = signal('');
  //#endregion


  //#region public methods

  //filter methods

  public onSearch() {
    if (this.filterForm.dirty) {
      this._paymentDetailsFilterQuery.set(this.getPaymentDetailFilter());
    }
  }

  public onReset() {
    if (this.filterForm.dirty) {
      this.filterForm.reset();
      this._paymentDetailsFilterQuery.set(this.getPaymentDetailFilter());
    }
  }


  //validation methods

  public isInvalidControl(controlName: string, isFilterForm: boolean = false): boolean {
    const form = isFilterForm ? this.filterForm : this.dialogModalForm;
    return this._validatorService.isInvalidField(form, controlName);
  }

  public getErrorMessage(controlName: string, isFilterForm: boolean = false, fieldNameToShow?: string): string | null {
    const form = isFilterForm ? this.filterForm : this.dialogModalForm;
    return this._validatorService.getFieldError(form, controlName, fieldNameToShow);
  }

  //dialog modal methods

  public showDialogModalOnCreateMode() {
    this._doGetAllRangePlus(() => {
      this.dialogModalForm.reset();
      this.dialogModalTitle.set('Create Payment Detail');
      this.dialogModal()?.showModal();
    },
      (err) => {
        this._toastService.error(ErrorUtilitiesClass.getErrorMessage(err), 'Error');
      }
    );
  }


  public showDialogModalOnUpdateMode(id: number) {
    if (!id) {
      this._toastService.error('Invalid Payment Detail Id');
      return;
    }
    const paymentDetail = this.paymentDetailPagedResult().results.find(a => a.id == id);
    if (!paymentDetail) {
      this._toastService.error('Payment Detail not found');
      return;
    }

    this._doGetAllRangePlus(() => {
      this.dialogModalTitle.set('Update payment detail');
      this.dialogModalForm.patchValue({
        ...paymentDetail,
        paymentDate: !paymentDetail.paymentDate ? null : this.getFormDateTime(paymentDetail.paymentDate)
      });
      this.dialogModal()?.showModal();
    });
  }

  public showPromptModalOnDeleteMode(id: number) {
    if (!id) {
      this._toastService.error('Invalid Payment Detail Id');
      return;
    }
    const paymentDetail = this.paymentDetailPagedResult().results.find(a => a.id == id);
    if (!paymentDetail) {
      this._toastService.error('Payment Detail not found');
      return;
    }

    this.deletePromptBodyText.set('Are you sure to delete the payment detail ?');
    this.paymentDetailIdToDelete.set(paymentDetail.id);
    this.isPromptDeleteSingleMode.set(true);
    this.promptModal()?.showModal();
  }


  public showPromptModalOnDeleteAllMode() {
    if (!this.paymentIdsSelectedForDelete() || !this.paymentIdsSelectedForDelete()?.length) {
      this._toastService.error('Please select at least one payment detail');
      return;
    }

    this.deletePromptBodyText.set('Are you sure to delete all the payments detail selected?');
    this.isPromptDeleteSingleMode.set(false);
    this.promptModal()?.showModal();
  }



  public deleteSelectedPaymentDetails() {
    this._paymentDetailService.deleteManyPaymentDetails(this.paymentIdsSelectedForDelete())
      .subscribe({
        next: () => {
          this._toastService.success('Payment details deleted successfully', 'Success');
          this._paymentDetailsFilterQuery.set(this.getPaymentDetailFilter());
          this.paymentIdsSelectedForDelete.set([]);
          this.promptModal()?.closeModal();
        },
        error: (err) => {
          this._toastService.error(ErrorUtilitiesClass.getErrorMessage(err), 'Error');
          this._paymentDetailsFilterQuery.set(this.getPaymentDetailFilter());
        }
      })
  }

  public deletePaymentDetail() {
    const id = this.paymentDetailIdToDelete();
    if (id == null) {
      this._toastService.error('Invalid Payment Detail Id');
      return;
    }

    this._paymentDetailService.deletePaymentDetail(id)
      .subscribe({
        next: () => {
          this._toastService.success('Payment detail deleted successfully', 'Success');
          this._paymentDetailsFilterQuery.set(this.getPaymentDetailFilter());
          this.paymentDetailIdToDelete.set(null);
          this.promptModal()?.closeModal();
        },
        error: (err) => {
          this._toastService.error(ErrorUtilitiesClass.getErrorMessage(err), 'Error');
        }
      })
  }

  public promptModalOkButtonClicked() {
    if (this.isPromptDeleteSingleMode()) {
      this.deletePaymentDetail();
    } else {
      this.deleteSelectedPaymentDetails();
    }
  }

  public onCancelPromptButtonClicked() {
    this.paymentDetailIdToDelete.set(null);
    this.promptModal()?.closeModal();
  }

  public onDialogModalOk() {
    const isCreation = !this.dialogModalForm.get('id')?.value;
    const value = this.dialogModalForm.value;
    const paymentDetail = {
      ...value,
      paymentDate: !value.paymentDate ? undefined : this.getFormDateTime(value.paymentDate)
    };

    const doTask = isCreation ? this._paymentDetailService.createPaymentDetail(paymentDetail as CreatePaymentDetailCommand)
      : this._paymentDetailService.updatePaymentDetail(paymentDetail as UpdatePaymentDetailCommand);

    doTask.subscribe({
      next: () => {
        this._toastService.success(isCreation ? 'Payment detail added successfully' : 'Payment detail updated successfully', 'Success');
        this._paymentDetailsFilterQuery.set(this.getPaymentDetailFilter());
        this.dialogModal()?.closeModal();
      },
      error: (error: any) => {
        this._toastService.error(ErrorUtilitiesClass.getErrorMessage(error), 'Error');
      }
    })
  }
  //Table methods
  public getRangePlusInfoById(id: number): string {
    const rangePlus = this.rangePlusList().find(rp => rp.id == id);
    return this.infoByRangePlus(rangePlus);
  }

  public infoByRangePlus(rangePlus?: RangePlusResponse): string {
    return rangePlus ? `${rangePlus.minExperience} - ${rangePlus.maxExperience} ($${rangePlus.plus})` : '';
  }

  public onPaymentChecked(event: Event, paymentId: number): void {

    const target = event.target as HTMLInputElement;
    if (!!target && target.checked !== undefined && this.payments().find(p => p.id == paymentId)) {
      if (target.checked) {
        this.paymentIdsSelectedForDelete.update(p => [...p, paymentId]);
      }
      else {
        this.paymentIdsSelectedForDelete.update(p => p.filter(p => p != paymentId));
      }
    }
  }



  //Pager methods

  public tableItemsPerPageChanged(itemsPerPage: number) {
    this.itemsPerPage.set(itemsPerPage);
    this._paymentDetailsFilterQuery.set(this.getPaymentDetailFilter());
  }

  public tablePageChanged(page: number) {
    this.page.set(page);
    this._paymentDetailsFilterQuery.set(this.getPaymentDetailFilter());
  }

  //#endregion

  //#region private methods

  getPaymentDetailFilter(): SearchPaymentDetailsByFilterQuery {
    const filterValue = this.filterForm.value;

    const cleaned = Object.entries(filterValue)
      .filter(([_, value]) =>
        value != null &&
        value !== undefined
      )
      .reduce((acc, [key, value]) => {
        let data: string | number = '';
        switch (key) {
          case 'fromPaymentDate':
          case 'toPaymentDate':
            data = this.getFormDateTime(value! as Date);
            break;
          case 'fromSalary':
          case 'toSalary':
            data = Number(value);
            break;
        }

        acc[key] = data;
        return acc;
      }, {} as any);

    return {
      ...cleaned,
      page: this.page(),
      pageSize: this.itemsPerPage(),
      requestCount: true
    }
  }


  doSearchByFilter(filter: SearchPaymentDetailsByFilterQuery) {
    if (untracked(() => this.isLoading())) {
      this._toastService.info('A request is already in progress. Please wait...');
      return;
    }
    this.isLoading.set(true);

    this._paymentDetailService.searchPaymentDetailsByQueryFilter(filter)
      .subscribe({
        next: (response) => {
          this.isLoading.set(false);
          this.paymentDetailPagedResult.set(response);

        },
        error: (err) => {
          this.isLoading.set(false);
          this._toastService.error(ErrorUtilitiesClass.getErrorMessage(err), 'Error');
        }
      })
  }

  private _doGetAllRangePlus(
    callbackNext?: () => void,
    callbackError?: (err: any) => void): void {

    this._rangePlusService.getAllRangePlus()
      .subscribe({
        next: (response) => {
          this.rangePlusList.set(response);
          callbackNext?.();
        },
        error: (err) => {
          callbackError?.(err);
        }
      })
  }

  private getFormDateTime(date: string | Date): string {
    const d = new Date(date);

    const local = new Date(d.getTime() - d.getTimezoneOffset() * 60000)
      .toISOString()
      .slice(0, 16);

    return local;
  }
}
