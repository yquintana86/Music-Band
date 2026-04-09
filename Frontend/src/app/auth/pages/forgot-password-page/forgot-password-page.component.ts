import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../../core/services/auth.service';
import { ValidatorsService } from '../../../shared/services/validator.service';
import { ToastrService } from 'ngx-toastr';
import { ErrorUtilitiesClass } from '../../../shared/interfaces/error-utilities.class';
import { Router, RouterLink } from '@angular/router';
import { FieldErrorDirective } from '../../../shared/directives/field-error-directive';

@Component({
  selector: 'forgot-password-page',
  imports: [ReactiveFormsModule, RouterLink, FieldErrorDirective],
  templateUrl: './forgot-password-page.component.html',
  styleUrl: './forgot-password-page.component.css',
})
export default class ResetPasswordPage {

  private _authenticationService = inject(AuthService);
  private _validatorService = inject(ValidatorsService);
  private _toastrService = inject(ToastrService);
  private _router = inject(Router);
  private _fb = inject(FormBuilder);

  forgotPasswordFormGroup = this._fb.group(
    {
      email: ['', [Validators.required, Validators.pattern(this._validatorService.emailPattern)]],
    });

    isFieldInvalid(field: string): boolean {
      return this._validatorService.isInvalidField(this.forgotPasswordFormGroup, field);
    }

    getErrorMessage(field: string): string {
      return this._validatorService.getFieldError(this.forgotPasswordFormGroup, field);
    }


  onSubmit(): void {
    if(this.forgotPasswordFormGroup.invalid)
    {
      this.forgotPasswordFormGroup.markAllAsTouched();
      return;
    }

    const email = this.forgotPasswordFormGroup.get('email')?.value;
    this._authenticationService.doForgotPassword(email!)
    .subscribe({
      next: () => {
        this._toastrService.success('Please check your email to reset your password', 'Success');
        this.forgotPasswordFormGroup.reset();
      },
      error: (err) => {
        this._toastrService.error(
          ErrorUtilitiesClass.getErrorMessage(err, 'Unable to reset password'),
          'Error'
        );
      }
    });
  }


}
