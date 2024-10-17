import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component, DestroyRef, ElementRef,
  inject,
  OnDestroy,
  signal,
  viewChild, viewChildren
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatError, MatFormField, MatHint, MatLabel, MatPrefix, MatSuffix } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { MatIcon } from '@angular/material/icon';
import {
  MatStep,
  MatStepContent,
  MatStepLabel,
  MatStepper, MatStepperIcon,
  MatStepperNext,
  MatStepperPrevious
} from '@angular/material/stepper';
import { FormArray, FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgxMaskDirective } from 'ngx-mask';
import { MatButton } from '@angular/material/button';
import { AuthorizationService } from '../../services/authorization.service';
import { asyncScheduler, catchError, filter, interval, map, of, Subject, switchMap, take, takeUntil, tap } from 'rxjs';
import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Router } from '@angular/router';


@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, MatFormField, MatInput, MatLabel, MatIcon, MatPrefix,
    MatSuffix, MatStepper, MatStep, MatStepLabel, MatStepContent,
    MatStepperPrevious, MatStepperNext, NgxMaskDirective,
    MatHint, ReactiveFormsModule, MatButton, MatError, MatStepperIcon
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [{
    provide: STEPPER_GLOBAL_OPTIONS, useValue: { displayDefaultIndicatorType: false }
  }]
})
export default class LoginComponent implements OnDestroy {

  #authS = inject(AuthorizationService);
  #router = inject(Router);
  #cdr = inject(ChangeDetectorRef);
  #destroyRef = inject(DestroyRef);

  private readonly resendVerificationCodeIntervalInSeconds = 60;
  private readonly verificationCodeLength = 6;

  protected matStepper = viewChild.required(MatStepper);
  protected verificationCodeInput = viewChildren<ElementRef<HTMLInputElement>>('verificationCodeInput');

  protected phoneNumberControl = new FormControl<string>('', { validators: [Validators.required], nonNullable: true });
  protected phoneNumberStepCompleted = signal(false);
  protected resendVerificationCodeBtnDisabled = signal(true);
  protected secondsLeftToResendVerificationCode = signal(this.resendVerificationCodeIntervalInSeconds);
  protected verificationCodeFormArray = new FormArray<FormControl<number | null>>([]);
  protected verificationCodeRequestInProcess = signal(false);

  #destroyIntervalSubscription$ = new Subject<void>()

  protected sendLoginRequest() {
    this.#destroyIntervalSubscription$.next();
    this.phoneNumberStepCompleted.set(false);
    const formattedPhoneNumber = '8' + this.phoneNumberControl.value;
    this.#authS.login$(formattedPhoneNumber)
      .pipe(
        tap(() => {
          this.phoneNumberStepCompleted.set(true);
          this.#cdr.detectChanges();
          this.moveToVerificationStep();
        })
      )
      .subscribe();
  }

  protected resendLoginRequest() {
    const formattedPhoneNumber = '8' + this.phoneNumberControl.value;
    this.#authS.login$(formattedPhoneNumber)
      .pipe(
        tap(() => this.startResendVerificationCodedInterval()))
      .subscribe();
  }

  protected pressKey(ev: KeyboardEvent) {
    if (this.phoneNumberControl.valid && ev.code === 'Enter') {
      this.sendLoginRequest();
    }
  }

  protected startResendVerificationCodedInterval() {
    this.resendVerificationCodeBtnDisabled.set(true);
    this.secondsLeftToResendVerificationCode.set(this.resendVerificationCodeIntervalInSeconds);
    this.#destroyIntervalSubscription$.next();

    interval(1000)
      .pipe(
        tap(() => this.secondsLeftToResendVerificationCode.update(secondsLeft => --secondsLeft)),
        take(this.resendVerificationCodeIntervalInSeconds),
        takeUntil(this.#destroyIntervalSubscription$)
      ).subscribe(v => {
        if (v === this.resendVerificationCodeIntervalInSeconds - 1) { this.resendVerificationCodeBtnDisabled.set(false); }
    });
  }

  private moveToVerificationStep() {
    this.matStepper().next();
    if (this.verificationCodeFormArray.length) {
      this.verificationCodeFormArray.reset();
    }
    this.startResendVerificationCodedInterval();
    if (!this.verificationCodeFormArray.controls.length) {
      this.initVerificationCodeFormArray();
    }
    asyncScheduler.schedule(() => this.verificationCodeInput()[0].nativeElement.focus())
  }

  private initVerificationCodeFormArray() {
    for (let i = 0; i < this.verificationCodeLength; i++) {
      this.verificationCodeFormArray.push(new FormControl<number | null>(null), { emitEvent: false });
    }
    this.listenForVerificationCodeSend();
  }

  private listenForVerificationCodeSend() {
    this.verificationCodeFormArray.valueChanges
      .pipe(
        filter(values => values.every(v => typeof v === 'number')),
        map(values => values.reduce((acc, digit) => acc += digit.toString(), '')),
        tap(() => {
          this.verificationCodeRequestInProcess.set(true);
          this.verificationCodeFormArray.disable({emitEvent: false});
        }),
        switchMap(verificationCode => this.#authS.sendVerificationCode(verificationCode)),
        tap( {
          next: () => this.#router.navigate(['main'], { relativeTo: null }),
          error: () => {
            this.verificationCodeRequestInProcess.set(false);
            this.verificationCodeFormArray.reset(undefined, { emitEvent: false });
            this.verificationCodeFormArray.enable({ emitEvent: false });
            this.listenForVerificationCodeSend();
          }
        })
      ).subscribe();
  }

  protected onPasteVerificationCode(event: ClipboardEvent, inputIndex: number) {
    event.preventDefault();
    const pastedText = event.clipboardData?.getData('text');
    if (!pastedText) { return; }
    const digitsToInsert = /^\s*?(\d{1,6})/.exec(pastedText)?.[1];
    if (!digitsToInsert) { return; }
    let currentInputIndex = inputIndex;
    for (let i = 0; i < digitsToInsert!.length && currentInputIndex < this.verificationCodeLength; i++, currentInputIndex++) {
      const currentControl =  this.verificationCodeFormArray.get(currentInputIndex.toString()) as FormControl<number>;
      currentControl.setValue(+digitsToInsert[i]);
      if (i === digitsToInsert.length - 1 || currentInputIndex === this.verificationCodeLength - 1) {
        const inputToFocusOn = this.verificationCodeInput()[currentInputIndex].nativeElement;
        inputToFocusOn.focus();
      }
    }
  }

  protected onKeyDownVerificationCode(event: KeyboardEvent, currentInputIndex: number) {
    const inputElement = event.target as HTMLInputElement;
    const currentValue = inputElement.valueAsNumber;
    //если вводим E или мат.символы, то скип
    if (event.code === 'KeyE' || event.code === 'Minus' || event.code === 'Equal') { event.preventDefault(); return; }
    //стрелочками меняем выбранный input. Если нажали BackSpace, то перемещаемся только, если в ячейке нет числа
    else if (event.code === 'ArrowRight') {
      currentInputIndex !== this.verificationCodeLength - 1 && this.verificationCodeInput()[currentInputIndex + 1].nativeElement.focus();
      return;
    } else if (event.code === 'ArrowLeft' || (event.code === 'Backspace' && !currentValue)) {
      if (currentInputIndex !== 0) {
        const inputToFocusOn = this.verificationCodeInput()[currentInputIndex - 1].nativeElement;
        inputToFocusOn.type = 'text';
        asyncScheduler.schedule(() => {
          inputToFocusOn.setSelectionRange(inputToFocusOn.value.length, inputToFocusOn.value.length);
          inputToFocusOn.focus();
          inputToFocusOn.type = 'number'
        });
      }
      return;
    }
    if (event.code.includes('Digit') && inputElement.value.length) {
      this.verificationCodeFormArray.get(currentInputIndex.toString())!.setValue(+event.key);
      event.preventDefault();
    }
    //если ввели число, то фокусируемся на следующей ячейке
    if (currentInputIndex !== this.verificationCodeLength - 1 && event.code.includes('Digit')
      && !event.shiftKey && !event.ctrlKey && !event.altKey && !event.metaKey) {
      asyncScheduler.schedule(() => this.verificationCodeInput()[currentInputIndex + 1].nativeElement.focus());
    }
    //если стёрли число, то фокусируемся на предыдущей ячейке
    asyncScheduler.schedule(() => {
      const newValue = inputElement.valueAsNumber;
      if (currentValue.toString() !== newValue.toString() && (!newValue && newValue !== 0) && currentInputIndex > 0) {
        this.verificationCodeInput()[currentInputIndex - 1].nativeElement.focus();
      }
    })
  }

  ngOnDestroy() {
    this.#destroyIntervalSubscription$.complete();
  }
}
