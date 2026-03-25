import { Injectable } from '@angular/core';
import { AbstractControl, FormGroup, ValidationErrors } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class ValidatorsService {

  public firstNameAndLastnamePattern: string = '([a-zA-Z]+) ([a-zA-Z]+)';
  public countryNamePattern: string = "^[A-Za-zÀ-ÖØ-öø-ÿ' -]{2,56}$";
  public musicalInstrumentNamePattern: string = "^[A-Za-zÀ-ÖØ-öø-ÿ' -]{2,60}$";
  public emailPattern: string = "^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$";

  constructor() { }

  public isInvalidField(formGroup: FormGroup, field: string): boolean {

    return (formGroup.contains(field) &&
      formGroup.get(field)?.errors &&
      formGroup.get(field)?.touched) as boolean;
  }

  public isInvalidControl(control: FormGroup): boolean {
    return (!!control && control.touched &&
      control.invalid) as boolean;
  }

  public getRawValidationError(formGroup: FormGroup, controlName: string): ValidationErrors | null {
    return !formGroup.contains(controlName) ? null : (formGroup.get(controlName)?.errors || null);
  }

  public getFieldError(formGroup: FormGroup, controlName: string, fieldNameToShow?: string): string {

    if (!!formGroup.contains(controlName)) {
      const errors = formGroup.get(controlName)?.errors;
      if (errors){
        const fieldName = fieldNameToShow ? fieldNameToShow : controlName;
        return this.#getError(errors, fieldName);
      }
    }

    return '';
  }

  public getControlErrors(control: AbstractControl): string {
    return (!!control && !!control.errors) ?
      this.#getError(control.errors!, 'control') : '';
  }


  public EqualPasswords(password: string, confirmPassword: string) {
    return (formGroup: AbstractControl): ValidationErrors | null => {
      const passwordValue = formGroup.get(password)?.value;
      const confirmPasswordValue = formGroup.get(confirmPassword)?.value;

      return (passwordValue !== confirmPasswordValue) ?
      { notEqualPasswords: true } : null;
    }
  }

  #getError(errors: ValidationErrors, field?: string): string {
    const keys = Object.keys(errors);
    for (const key of keys) {
      switch (key) {
        case 'required': {
          return `The ${field} field is required`;
        }
        case 'minlength': {
          return `The ${key} must be more than ${errors!['minlength'].requiredLength} characters`;
        }
        case 'pattern': {
          return `The ${field} field must be valid`;
        }
        case 'notEqualPasswords': {
          return `Passwords are not equal`;
        }
        case 'min': {
          return `The ${field} field must be more than ${errors!['min'].min}`;
        }
        case 'max': {
          return `The ${field} field must be less than ${errors!['max'].max}`;
        }
        default:
          return `must be tested`;
      }
    }

    return '';
  }

}
