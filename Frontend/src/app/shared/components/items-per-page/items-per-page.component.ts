import { Component, input, output } from '@angular/core';

@Component({
  selector: 'items-per-page',
  imports: [],
  templateUrl: './items-per-page.component.html',
  styleUrl: './items-per-page.component.css',
})
export class ItemsPerPageComponent {

  itemsPerPage = input<number>(20 | 40 | 60 | 80 | 100);
  itemsPerPageChange = output<number>();

  onItemsPerPageChange(itemsPerPage: string) {
    this.itemsPerPageChange.emit(parseInt(itemsPerPage));
  }

  get itemsPerPageOptions(): number[] {
    return [20, 40, 60, 80, 100];
  }

}
