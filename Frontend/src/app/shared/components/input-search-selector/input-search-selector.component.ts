import { Component, computed, effect, ElementRef, inject, input, linkedSignal, output, signal, viewChild, WritableSignal } from '@angular/core';
import { APIOperationResult, DtoWithId, SelectItem, PagedResult } from '../../interfaces';
import { HttpClient } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { ErrorUtilitiesClass } from '../../interfaces/error-utilities.class';
import { PagingFilter } from '../../interfaces/paging-filter.interface';
import { map } from 'rxjs';

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
  public initialDto = input<SelectItem | null>(null);
  public callBackShowFieldValueFn = input.required<(item: DtoWithId) => SelectItem>();
  public callBackGetFilterFn = input.required<(query: string) => PagingFilter>();
  public elementSelected = output<number>();
  public elementCleared = output<void>();


   public idSelected: WritableSignal<number | null> = signal(null);
  // public idSelected: WritableSignal<number | null> = linkedSignal(() => {
  //   const dto = this.initialDto();

  //   if(!dto){
  //     (this.inputSearch()?.nativeElement as HTMLInputElement).value = '';
  //     return null;
  //   }
  //   (this.inputSearch()?.nativeElement as HTMLInputElement).value = dto.text;
  //   return dto.id;
  // });

  public itemSelected: WritableSignal<SelectItem | null> = linkedSignal(() => {
    const dto = this.initialDto();
    return !dto ? null : { id: dto.id, text: dto.text };
  });

  selectedItem = effect(() => {
    const item = this.itemSelected();

    const input = this.inputSearch()?.nativeElement as HTMLInputElement;
    if(!item)
    {
      this.idSelected.set(null);
      if(input) input.value = '';
      return;
    }
    this.idSelected.set(item.id);
    if(input) input.value = item.text;
  });

  public xHidden = computed(() => !this.itemSelected());
  public dropDownHidden = signal(true);
  public foundedItems: WritableSignal<SelectItem[]> = signal([]);


  public doSearch(value: string) {
    if(!value) return;
    this._httpClient.post<APIOperationResult<PagedResult<DtoWithId>>>(
      this.searchPostUri(), this.callBackGetFilterFn()(value)
    )
    .pipe(
      map(response => response.data.results),
      map(items => items.map(item => this.callBackShowFieldValueFn()(item)))
    )
    .subscribe({
      next: (items) => {
        this.foundedItems.set(items);
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
      (this.inputSearch()?.nativeElement as HTMLInputElement).value = item.text;
      this.itemSelected.set(item);
      this.elementSelected.emit(item.id);
      this.closeDropDown();
    }
  }



}
