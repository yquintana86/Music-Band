import { Component, input, output } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'filter-layout',
  imports: [ReactiveFormsModule],
  templateUrl: './filter-layout.component.html',
  styleUrl: './filter-layout.component.css',
})
export class FilterLayoutComponent {

  filterTitle = input<string>('Filter');
  formGroup = input.required<FormGroup>();
  reset = output<void>();
  search = output<void>();


  onClear(): void {
      this.reset.emit();

  }

  onSearch(): void {
    this.search.emit();
}
}
