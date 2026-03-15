import { Component, ElementRef, input, output, viewChild } from '@angular/core';

@Component({
  selector: 'dialog-modal',
  imports: [],
  templateUrl: './dialog-modal.component.html',
  styleUrl: './dialog-modal.component.css',
})
export class DialogModalComponent {

  dialogModal = viewChild<ElementRef<DialogModalComponent>>('dialogModal');
  closeButtonModal = viewChild<ElementRef<HTMLElement>>('closeBtn');
  okButtonEmit = output<void>();
  cancelButtonEmit = output<void>();
  closeButtonEmit = output<void>();

  title = input<string>('Hello!');
  divModalBoxClasses = input<string>('modal-box w-11/12 max-w-5xl p-0 border-2 border-(--color-primary)');
  divContentClasses = input<string>('h-40 bg-surface');
  okButtonText = input<string>('Ok');
  cancelButtonText = input<string>('Cancel');
  closeButtonText = input<string>('Close');
  showOkButton = input<boolean>(true);
<<<<<<< HEAD
=======
  disableOKButton = input<boolean>(false);
>>>>>>> feature/dev
  showCloseButton = input<boolean>(true);
  showCancelButton = input<boolean>(true);


  showModal() {
     this.dialogModal()?.nativeElement.showModal();
  }

  closeModal() {
    this.closeButtonModal()?.nativeElement.click();
  }

  okClicked() {
    this.okButtonEmit.emit();
  }

  cancelClicked() {
    this.cancelButtonEmit.emit();
  }

}
