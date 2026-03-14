import { Directive, effect, ElementRef, input, Input } from '@angular/core';
@Directive({
  selector: '[fielderror]',
})
export class FieldErrorDirective {

  isInvalidField = input<boolean>(false);
  error = input<string | null>(null);
  color = input<string | null>(null);

  constructor(private el?: ElementRef<HTMLElement>) {
    effect((onCleanup) => {
      if(!!el){
        if(this.isInvalidField())
        {
          this.el!.nativeElement.style.color = this.color() || 'red';
          this.el!.nativeElement.innerHTML = this.error() || '';
          return;
        }
        this.el!.nativeElement.innerHTML = '';
      }
    })
   }
}
