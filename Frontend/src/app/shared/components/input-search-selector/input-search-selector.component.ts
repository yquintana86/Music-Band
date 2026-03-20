import { Component, computed, ElementRef, inject, input, linkedSignal, output, signal, viewChild, WritableSignal } from '@angular/core';
import { APIOperationResult, DtoWithId, InitialDtoWithId, PagedResult } from '../../interfaces';
import { HttpClient } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { ErrorUtilitiesClass } from '../../interfaces/error-utilities.class';
import { PagingFilter } from '../../interfaces/paging-filter.interface';

@Component({
  selector: 'input-search-selector',
  imports: [],
  templateUrl: './input-search-selector.component.html',
  styleUrl: './input-search-selector.component.css',
})
export class InputSearchSelectorComponent {

  private readonly inputSearch = viewChild<ElementRef<HTMLInputElement>>('inputSearch');

  private readonly _httpClient = inject(HttpClient);
  private readonly _toastService = inject(ToastrService);

  public searchPostUri = input.required<string>();
  public placeholder = input<string>('Search...');
  public searchButtonTitle = input<string>('Search');
  public initialDto = input<InitialDtoWithId | null>(null);
  public callBackShowFieldValueFn = input.required<(item: DtoWithId) => string>();
  public callBackGetFilterFn = input.required<(query: string) => PagingFilter>();
  public elementSelected = output<DtoWithId>();
  public elementCleared = output<void>();


  public idSelected: WritableSignal<number | null> = linkedSignal(() => {
    const dto = this.initialDto();

    if(!dto){
      (this.inputSearch()?.nativeElement as HTMLInputElement).value = '';
      return null;
    }
    (this.inputSearch()?.nativeElement as HTMLInputElement).value = dto.text;
    return dto.id;
  });

  public itemSelected: WritableSignal<DtoWithId | null> = linkedSignal(() => {
    const dto = this.initialDto();

    if(!dto){
      (this.inputSearch()?.nativeElement as HTMLInputElement).value = '';
      return null;
    }
    (this.inputSearch()?.nativeElement as HTMLInputElement).value = dto.text;
    return { id: dto.id };
  });


  public xHidden = computed(() => !this.itemSelected());
  public dropDownHidden = signal(true);
  public foundedItems: WritableSignal<DtoWithId[]> = signal([]);


  public doSearch(value: string) {
    if(!value) return;
    this._httpClient.post<APIOperationResult<PagedResult<DtoWithId>>>(this.searchPostUri(), this.callBackGetFilterFn()(value))
    .subscribe({
      next: (result) => {
        this.foundedItems.set(result.data.results);
        this.clearSelection();
        this.dropDownHidden.set(false);
      },error: (err) => {
        this._toastService.error(ErrorUtilitiesClass.getErrorMessage(err), 'Error');
        this.dropDownHidden.set(true);
      }
    });
  }

  public changeSelected(itemId: number){
    const item = this.foundedItems().find(i => i.id == itemId);
    this.idSelected.set( item ? item.id : null);
  }

  public clearSelection(){
    this.idSelected.set(null);
    this.itemSelected.set(null);
    (this.inputSearch()?.nativeElement as HTMLInputElement).value = '';
    this.elementCleared.emit();
    this.closeDropDown();
  }

  public closeDropDown(){
    this.dropDownHidden.set(true);
  }

  public selectItem(){
    const item = this.foundedItems().find(i => i.id == this.idSelected());
    if(item){
      const inputValue = this.callBackShowFieldValueFn()(item);
      (this.inputSearch()?.nativeElement as HTMLInputElement).value = inputValue;
      this.itemSelected.set(item);
      this.elementSelected.emit(item);
      this.closeDropDown();
    }
  }



}
