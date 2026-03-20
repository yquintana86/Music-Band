import { Component, input, output } from '@angular/core';

@Component({
  selector: 'table-layout',
  imports: [],
  templateUrl: './table.component.html',
  styleUrl: './table.component.css',
})
export class TableComponent {

  addButtonAlternative = input<string>('');
  deleteButtonAlternative = input<string>('');
  tableTitle = input<string>('');
  addButtonClicked = output<void>();
  deleteAllButtonClicked = output<void>();
  deleteAllButtonDisabled = input<boolean>(false);

  onAdd() {
    this.addButtonClicked.emit();
  }
  onDelete() {
    this.deleteAllButtonClicked.emit();
  }

  get addButtonAlternativeText(): string{
    return `Add ${this.addButtonAlternative()}`
  }

  get deleteButtonAlternativeText(): string{
    return `Delete selected ${this.addButtonAlternative()}`
  }

  get tableTitleText(): string{
    return !this.tableTitle() ?  'List' : this.tableTitle();
  }
}
