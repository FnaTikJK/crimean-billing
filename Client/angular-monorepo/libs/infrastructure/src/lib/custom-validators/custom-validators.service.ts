import { inject, Injectable, NgZone } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AbstractControl, FormGroup, ValidatorFn } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class MyValidatorsService {

  shouldNotMatchValidator(firstControlName: string, secondControlName: string, skipFalsyValue = true): ValidatorFn {
    return (formGroup: AbstractControl) => {
      const firstControl = formGroup.get(firstControlName);
      const secondControl = formGroup.get(secondControlName);

      if (!firstControl || !secondControl) { return null; }

      const firstControlErrors = firstControl.errors ?? {};
      const secondControlErrors = secondControl.errors ?? {};

      if (skipFalsyValue && (!firstControl.value || !secondControl.value)) {
        delete firstControlErrors['shouldNotMatch']; delete secondControlErrors['shouldNotMatch'];
        firstControl.setErrors(Object.keys(firstControlErrors).length ? { ...firstControlErrors} : null);
        secondControl.setErrors(Object.keys(secondControlErrors).length ? { ...secondControlErrors} : null);
        return null;
      }

      if (firstControl.value === secondControl.value) {
        firstControl.setErrors({ ...firstControlErrors, shouldNotMatch: true });
        secondControl.setErrors({ ...secondControlErrors, shouldNotMatch: true });
        return { shouldNotMatch: true };
      } else {
        delete firstControlErrors['shouldNotMatch']; delete secondControlErrors['shouldNotMatch'];
        firstControl.setErrors(Object.keys(firstControlErrors).length ? { ...firstControlErrors} : null);
        secondControl.setErrors(Object.keys(secondControlErrors).length ? { ...secondControlErrors} : null);
        return null;
      }
    };
  }
}
