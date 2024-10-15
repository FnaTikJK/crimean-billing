import { ChangeDetectionStrategy, Component, inject, viewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatError, MatFormField, MatHint, MatLabel, MatPrefix, MatSuffix } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { MatIcon } from '@angular/material/icon';
import {
  MatStep,
  MatStepContent,
  MatStepLabel,
  MatStepper,
  MatStepperNext,
  MatStepperPrevious
} from '@angular/material/stepper';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgxMaskDirective } from 'ngx-mask';
import { MatButton } from '@angular/material/button';
import { AuthorizationService } from '../../services/authorization.service';
import { tap } from 'rxjs';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, MatFormField, MatInput, MatLabel, MatIcon, MatPrefix,
    MatSuffix, MatStepper, MatStep, MatStepLabel, MatStepContent,
    MatStepperPrevious, MatStepperNext, NgxMaskDirective,
    MatHint, ReactiveFormsModule, MatButton, MatError
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class LoginComponent {

  private authS = inject(AuthorizationService);

  protected matStepper = viewChild.required(MatStepper);

  protected phoneNumberControl = new FormControl<string>('', { validators: [Validators.required], nonNullable: true });

  protected sendLoginRequest() {
    const formattedPhoneNumber = '8' + this.phoneNumberControl.value;
    this.authS.login$(formattedPhoneNumber)
      .pipe(
        tap(() => this.matStepper().next())
      )
      .subscribe();
  }

  protected pressKey(ev: KeyboardEvent) {
    if (this.phoneNumberControl.valid && ev.code === 'Enter') {
      this.sendLoginRequest();
    }
  }
}
