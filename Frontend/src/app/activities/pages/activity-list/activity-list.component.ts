import { Component, computed, effect, inject, signal, untracked, viewChild } from '@angular/core';
import { ActivityService } from '../../services/activity.service';
import { ValidatorsService } from '../../../shared/services/validator.service';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivityFilterQuery, ActivityReponse, CreateActivityCommand, UpdateActivityCommand } from '../../interfaces';
import { PagedResult } from '../../../shared/interfaces';
import { ErrorUtilitiesClass } from '../../../shared/interfaces/error-utilities.class';
import { DialogModalComponent } from '../../../shared/components/dialog-modal/dialog-modal.component';
import { FilterLayoutComponent } from "../../../shared/components/filter-layout/filter-layout.component";
import { toSignal } from '@angular/core/rxjs-interop';
import { map, startWith } from 'rxjs';
import { FieldErrorDirective } from '../../../shared/directives/field-error-directive';
import { TableComponent } from "../../../shared/components/table/table.component";
import { DatePipe } from '@angular/common';
import { ItemsPerPageComponent } from "../../../shared/components/items-per-page/items-per-page.component";
import { PagerComponent } from "../../../shared/components/pager/pager.component";



@Component({
  selector: 'app-activity-list',
  imports: [ReactiveFormsModule, FilterLayoutComponent, DialogModalComponent, FieldErrorDirective, TableComponent, DatePipe, ItemsPerPageComponent, PagerComponent],
  templateUrl: './activity-list.component.html',
  styleUrl: './activity-list.component.css',
})
export default class ActivityListComponent {

  private _activityService = inject(ActivityService);
  private _validatorService = inject(ValidatorsService);
  private _toastService = inject(ToastrService);
  private _fb = inject(FormBuilder);
  //#region Private Fields
  private _activityFilterQuery = signal<ActivityFilterQuery>({
    page: 1,
    pageSize: 20,
    requestCount: true,
  });

  pageCount = computed(() => this.activitiesPagedResult().pageCount || 1);

  private activityFilterQueryEffect = effect(() => {
    const activityFilter = this._activityFilterQuery();
    this.doSearchByFilter(activityFilter);
  });

  private activityIdToDelete = signal<number | null>(null);

  //#endregion

  //#region public fields
  public isLoading = signal(false);

  public activitiesPagedResult = signal<PagedResult<ActivityReponse>>({
    currentpage: 1,
    pageSize: 20,
    itemCount: 20,
    hasNextPage: false,
    displayFrom: 1,
    displayTo: 1,
    results: [],
  });

  public activities = computed(() => this.activitiesPagedResult().results);

  public page = signal(1);
  public itemsPerPage = signal(20);

  public filterForm = this._fb.group({
    international: this._fb.control<string | null>(null),
    begin: this._fb.control<string | null>(null),
    end: this._fb.control<string | null>(null),
  });

  public dialogModalForm = this._fb.group({
    id: this._fb.control<number | null>(null),
    name: ['', [Validators.required, Validators.maxLength(50)]],
    client: ['', [Validators.required, Validators.maxLength(50)]],
    description: [''],
    international: ['true', [Validators.required]],
    begin: this._fb.control<string | null>(null, [Validators.required]),
    end: this._fb.control<string | null>(null, [Validators.required]),
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
    this._activityFilterQuery.set(this.getActivityFilter());
  }

  public onReset() {
    if (this.filterForm.dirty) {
      this.filterForm.reset();
      this._activityFilterQuery.set(this.getActivityFilter());
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
    this.dialogModalTitle.set('Create Activity');
    this.dialogModalForm.reset();
    this.dialogModal()?.showModal();
  }


  public showDialogModalOnUpdateMode(id: number) {
    if (!id) {
      this._toastService.error('Invalid Activity Id');
      return;
    }
    const activity = this.activitiesPagedResult().results.find(a => a.id == id);
    if (!activity) {
      this._toastService.error('Activity not found');
      return;
    }

    this.dialogModalTitle.set('Update Activity');
    this.dialogModalForm.patchValue({
      ...activity,
      international: activity.international ? 'true' : 'false',
      begin: activity.begin ? this.getFormDate(activity.begin) : null,
      end: activity.end ? this.getFormDate(activity.end) : null
    });
    this.dialogModal()?.showModal();
  }

  public showPromptModalOnDeleteMode(id: number) {
    if (!id) {
      this._toastService.error('Invalid Activity Id');
      return;
    }
    const activity = this.activitiesPagedResult().results.find(a => a.id == id);
    if (!activity) {
      this._toastService.error('Activity not found');
      return;
    }

    this.deletePromptBodyText.set(`Are you sure to delete the '${activity.name}' activity ?`);
    this.activityIdToDelete.set(activity.id);
    this.promptModal()?.showModal();
  }

  public onDeleteAllButtonClicked() {

  }

  public onCancelPromptButtonClicked() {
    this.activityIdToDelete.set(null);
    this.promptModal()?.closeModal();
  }

  public promptModalOkButtonClicked() {
    debugger;
    const id = this.activityIdToDelete();
    if (id == null) {
      this._toastService.error('Invalid Activity Id');
      return;
    }

    this._activityService.deleteActivity(id)
      .subscribe({
        next: () => {
          this._toastService.success('Activity deleted successfully', 'Success');
          this._activityFilterQuery.set(this.getActivityFilter());
          this.activityIdToDelete.set(null);
          this.promptModal()?.closeModal();
        },
        error: (err) => {
          this._toastService.error(ErrorUtilitiesClass.getErrorMessage(err), 'Error');
          this.activityIdToDelete.set(null);
          this.promptModal()?.closeModal();
        }
      })
  }

  public onDialogModalOk() {
    const isCreation = !this.dialogModalForm.get('id')?.value;
    const value = this.dialogModalForm.value;
    const activity = {...value, international: value.international == 'true'};

    const doTask = isCreation ? this._activityService.createActivity(activity as CreateActivityCommand)
                              : this._activityService.updateActivity(activity as UpdateActivityCommand);

    doTask.subscribe({
      next: () => {
        this._toastService.success(isCreation ? 'Activity added successfully' : 'Activity updated successfully', 'Success');
        this._activityFilterQuery.set(this.getActivityFilter());
        this.dialogModal()?.closeModal();
      },
      error: (error: any) => {
        this._toastService.error(ErrorUtilitiesClass.getErrorMessage(error), 'Error');
      }
    })
  }

  //Pager methods

  public tableItemsPerPageChanged(itemsPerPage: number) {
    this.itemsPerPage.set(itemsPerPage);
    this._activityFilterQuery.set(this.getActivityFilter());
  }

  public tablePageChanged(page: number) {
    this.page.set(page);
    this._activityFilterQuery.set(this.getActivityFilter());
  }

  //#endregion

  //#region private methods

  getActivityFilter(): ActivityFilterQuery {
    const filterValue = this.filterForm.value;

    const cleaned = Object.entries(filterValue)
      .filter(([_, value]) =>
        value != null &&
        value !== undefined &&
        value !== '')
      .reduce((acc, [key, value]) => {

        let data: string | boolean = '';
        switch (key){
          case 'begin':
            data = this.getFormDate(value!);
            break;
          case 'end':
            data = this.getFormDate(value!);
            break;
          case 'international':
            data = value == 'true';
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


  doSearchByFilter(activityFilter: ActivityFilterQuery) {
    if (untracked(() => this.isLoading())) {
      this._toastService.info('A request is already in progress. Please wait...');
      return;
    }
    this.isLoading.set(true);

    this._activityService.getActivitiesByFilterQuery(activityFilter)
      .subscribe({
        next: (response) => {
          this.isLoading.set(false);
          this.activitiesPagedResult.set(response);
        },
        error: (err) => {
          this.isLoading.set(false);
          this._toastService.error(ErrorUtilitiesClass.getErrorMessage(err), 'Error');
        }
      })
  }

  private getFormDate(date: string | Date): string {
    return new Date(date).toISOString().split('T')[0];
  }

  //#endregion
}
