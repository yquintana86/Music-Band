import { Component, computed, input, linkedSignal, output } from '@angular/core';
import { CheckedItem } from '../../interfaces';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'select-item',
  imports: [CommonModule],
  templateUrl: './select-item.component.html',
  styleUrl: './select-item.component.css',
})
export class SelectItemComponent {

  public inputList = input<CheckedItem[]>([]);
  public showButtons = input<boolean>(false);
  public isReadOnly = input<boolean>(false);
  public selectionChange = output<CheckedItem[]>();

  public existCheckedItem = computed(() => this.inputList().some(i => i.checked));

  public updateSelection(itemId: number, event: Event)
  {
    const target = event.target as HTMLInputElement;
    const updated = this.inputList().map(i => i.id === itemId ? {...i, checked: target.checked} : i);
    this.selectionChange.emit(updated);
}
  public clearAllSelection()
  {
    const updated = this.inputList().map(i => ({...i, checked: false}));
    this.selectionChange.emit(updated);
  }

}
