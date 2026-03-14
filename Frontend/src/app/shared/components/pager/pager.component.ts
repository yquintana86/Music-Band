import { Component, input, output, computed } from '@angular/core';

@Component({
  selector: 'table-pager',
  imports: [],
  templateUrl: './pager.component.html',
  styleUrl: './pager.component.css',
})
export class PagerComponent {
  currentPage = input.required<number>();
  pageCount = input.required<number>();
  pageChanged = output<number>();
  pagerList = computed(() => {
    return Array.from({length: this.pageCount()}, (_, i) => i + 1);
  });

  onPagerClick(page: number){
    this.pageChanged.emit(page);
  }

  onPagerNext(){
    if(this.currentPage() < this.pageCount()){
      this.pageChanged.emit(this.currentPage() + 1);
    }
  }

  onPagerPrevious(){
    if(this.currentPage() > 1){
      this.pageChanged.emit(this.currentPage() - 1);
    }
  }
}
