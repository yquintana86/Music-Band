import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';

import { ToastrService } from 'ngx-toastr';

import { ValidatorsService } from '../../../shared/services/validator.service';
import { AuthService } from '../../../core/services/auth.service';
import { FieldErrorDirective } from '../../../shared/directives/field-error-directive';
import { ErrorUtilitiesClass } from '../../../shared/interfaces/error-utilities.class';

@Component({
  selector: 'app-register-page',
  imports: [FieldErrorDirective, ReactiveFormsModule, RouterLink],
  templateUrl: './register-page.component.html',
  styleUrl: './register-page.component.css',
})
export default class RegisterPage  {

  #validatorService = inject(ValidatorsService);
  #fb = inject(FormBuilder);
  #authService = inject(AuthService);
  #toastrService = inject(ToastrService);

  registerFormGropup = this.#fb.group({
    firstName: ['', [Validators.required, Validators.minLength(3)]],
    lastName: ['', [Validators.required, Validators.minLength(3)]],
    email: ['', [Validators.required, Validators.pattern(this.#validatorService.emailPattern)]],
    password: ['', [Validators.required, Validators.minLength(3)]],
    confirmPassword: ['', [Validators.required, Validators.minLength(3)]],
  },
  {
    validators: [
      this.#validatorService.EqualPasswords('password', 'confirmPassword')
    ]
  });

  //Private Methods

  private doRegister(): void{
    this.#authService.register({
      email: this.registerFormGropup.get('email')?.value ?? '',
      password: this.registerFormGropup.get('password')?.value ?? '',
      firstName: this.registerFormGropup.get('firstName')?.value ?? '',
      lastName: this.registerFormGropup.get('lastName')?.value ?? '',
    }).subscribe({
      next: () => {
        this.#toastrService.success('User registered successfully', 'Success');
      },
      error: (err) => {
        this.#toastrService.error(
          ErrorUtilitiesClass.getErrorMessage(err, 'User not registered'),
          'Error'
        );
      }
    });
  }



  // Public Methods

  public isInvalidField(field: string): boolean {
    return this.#validatorService.isInvalidField(this.registerFormGropup, field);
  }

  public getErrorMessage(field: string, fieldNameToShow?: string): string {
    return this.#validatorService.getFieldError(this.registerFormGropup, field, fieldNameToShow);
  }

  public getFormGroupErrors(): string {
    return this.#validatorService.getControlErrors(this.registerFormGropup);
  }

  public isFormGroupInvalid(): boolean {
    return this.#validatorService.isInvalidControl(this.registerFormGropup);
  }

  public onSubmit(): void {
    if(this.registerFormGropup.invalid)
    {
      this.registerFormGropup.markAllAsTouched();
      return;
    }

    this.doRegister();
  }

}
