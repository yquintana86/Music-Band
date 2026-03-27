import { Component, viewChild, ElementRef, OnInit, WritableSignal, signal, input, output, AfterViewInit } from '@angular/core';
import { catchError, EMPTY, fromEvent, map } from 'rxjs';

@Component({
  selector: 'input-search',
  imports: [],
  templateUrl: './input-search.component.html',
  styleUrl: './input-search.component.css',
})
export class InputSearchComponent implements AfterViewInit {

  public searchPlaceholder = input<string>('Search...');
  public searchButtonTitle = input<string>('Search');
  public isParentSearching = input<boolean>(false);
  public clearClicked = output<void>();
  public searchClicked = output<string>();

  private readonly inputSearch = viewChild<ElementRef<HTMLInputElement>>("inputSearch");
  public readonly inputHasValue: WritableSignal<boolean> = signal(false);


  ngAfterViewInit(): void {
    const inputSearch = this.inputSearch()?.nativeElement as HTMLInputElement;
     fromEvent(inputSearch, 'input')
     .pipe(
        map((event: Event) => {
          const value = (event.target as HTMLInputElement).value;
          this.inputHasValue.set(value.length > 0);
        }),
        catchError((err) => {
          console.error(err);
          return EMPTY;
        })
     ).subscribe();
  }

  public clearSelection(){
    const inputSearch = this.inputSearch()?.nativeElement as HTMLInputElement;

    if(inputSearch && inputSearch.value)
    {
      inputSearch.value = '';
      this.inputHasValue.set(false);
      this.clearClicked.emit();
    }
  }

  public doSearch(query: string){
    if(!query) return;
    this.searchClicked.emit(query);
  }

}
