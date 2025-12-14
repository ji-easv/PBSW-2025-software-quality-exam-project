import { AbstractControl } from "@angular/forms";

export function positiveNumberValidator(control: AbstractControl) {
  const value = Number.parseInt(control.value);
  if (value > 0) {
    return null;
  }
  return { positiveNumber: false };
}
