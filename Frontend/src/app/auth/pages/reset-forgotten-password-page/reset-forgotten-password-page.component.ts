import { Component, inject, OnInit, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../../core/services/auth.service';
import { FieldErrorDirective } from '../../../shared/directives/field-error-directive';
import { ErrorUtilitiesClass } from '../../../shared/interfaces/error-utilities.class';
import { ValidatorsService } from '../../../shared/services/validator.service';

@Component({
  imports: [FieldErrorDirective, ReactiveFormsModule, RouterLink],
  templateUrl: './reset-forgotten-password-page.component.html',
  styleUrl: './reset-forgotten-password-page.component.css',
})
export default class ResetForgottenPasswordPageComponent implements OnInit  {
  private _validatorService = inject(ValidatorsService);
  private _activatedRoute = inject(ActivatedRoute);
  private _router = inject(Router);
  private _fb = inject(FormBuilder);
  private _authService = inject(AuthService);
  private _toastrService = inject(ToastrService);


  ngOnInit(): void {
    const token = this._activatedRoute.snapshot.queryParams['token'];
    if(!token){
      this._router.navigate(['login']);
      return;
    }
    this.resetForgottenPasswordFormGropup.get('token')?.setValue(token);

  }

  resetForgottenPasswordFormGropup = this._fb.group({
    email: ['', [Validators.required, Validators.pattern(this._validatorService.emailPattern)]],
    password: ['', [Validators.required, Validators.minLength(3)]],
    confirmPassword: ['', [Validators.required, Validators.minLength(3)]],
    token: ['', [Validators.required]]
  },
  {
    validators: [
      this._validatorService.EqualPasswords('password', 'confirmPassword')
    ]
  });

  //Private Methods

  private doResetForgottenPassword(): void{
    this._authService.doResetForgottenPassword({
      email: this.resetForgottenPasswordFormGropup.get('email')?.value ?? '',
      password: this.resetForgottenPasswordFormGropup.get('password')?.value ?? '',
      token: this.resetForgottenPasswordFormGropup.get('token')?.value ?? '',
    }).subscribe({
      next: () => {
        this._toastrService.success('Password reset successfully', 'Success');
      },
      error: (err) => {
        this._toastrService.error(
          ErrorUtilitiesClass.getErrorMessage(err, 'Unable to reset password'),
          'Error'
        );
      }
    });
  }



  // Public Methods

  public isInvalidField(field: string): boolean {
    return this._validatorService.isInvalidField(this.resetForgottenPasswordFormGropup, field);
  }

  public getErrorMessage(field: string, fieldNameToShow?: string): string {
    return this._validatorService.getFieldError(this.resetForgottenPasswordFormGropup, field, fieldNameToShow);
  }

  public getFormGroupErrors(): string {
    return this._validatorService.getControlErrors(this.resetForgottenPasswordFormGropup);
  }

  public isFormGroupInvalid(): boolean {
    return this._validatorService.isInvalidControl(this.resetForgottenPasswordFormGropup);
  }

  public onSubmit(): void {
    if(this.resetForgottenPasswordFormGropup.invalid)
    {
      this.resetForgottenPasswordFormGropup.markAllAsTouched();
      return;
    }

    this.doResetForgottenPassword();
  }
}
