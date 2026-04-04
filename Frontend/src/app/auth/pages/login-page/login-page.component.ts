import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from "@angular/router";

import { ToastrService } from 'ngx-toastr';

import { ValidatorsService } from '../../../shared/services/validator.service';
import { AuthService } from '../../../core/services/auth.service';
import { FieldErrorDirective } from '../../../shared/directives/field-error-directive';
import { ErrorUtilitiesClass } from '../../../shared/interfaces/error-utilities.class';

@Component({
  selector: 'app-login-page',
  imports: [RouterLink, ReactiveFormsModule, FieldErrorDirective],
  templateUrl: './login-page.component.html',
  styleUrl: './login-page.component.css',
})
export default class LoginPageComponent {

  #fb = inject(FormBuilder);
  #validatorService = inject(ValidatorsService);
  #authenticationService = inject(AuthService);
  #toastService = inject(ToastrService);
  #router = inject(Router);
  #activatedRoute = inject(ActivatedRoute);
  returnUrlQueryParams = this.#activatedRoute.snapshot.queryParams['returnUrl'] || '/dashboard';

  loginFormGroup = this.#fb.group({
    email: ['', [Validators.required, Validators.pattern(this.#validatorService.emailPattern)]],
    password: ['', [Validators.required, Validators.minLength(3)]]
  });


  // Private methods
  #doSignIn(): void {
    const { email = '', password = '' } = this.loginFormGroup.value;

    this.#authenticationService.login(email!, password!)
      .subscribe(
        {
          next: () => {
            this.#router.navigateByUrl(this.returnUrlQueryParams);
          },
          error: (err) => {
            this.#toastService.error(ErrorUtilitiesClass.getErrorMessage(err), 'Error');
          }
        }
      );
  }



  // Public methods

  isFieldInvalid(field: string): boolean {
    return this.#validatorService.isInvalidField(this.loginFormGroup, field);
  }

  getErrorMessage(field: string) {
    return this.#validatorService.getFieldError(this.loginFormGroup, field);
  }

  public onSubmit(): void {

    if (this.loginFormGroup.invalid) {
      this.loginFormGroup.markAllAsTouched();
      return;
    }

    this.#doSignIn();
  }


}
