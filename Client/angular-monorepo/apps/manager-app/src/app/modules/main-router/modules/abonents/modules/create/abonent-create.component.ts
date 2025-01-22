import { ChangeDetectionStrategy, ChangeDetectorRef, Component, DestroyRef, inject } from "@angular/core";
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { BehaviorSubject, catchError, combineLatest, distinctUntilChanged, forkJoin, map, of, switchMap, take, tap } from "rxjs";
import { CommonModule } from "@angular/common";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { ActivatedRoute, Router } from "@angular/router";
import { MatButtonModule } from "@angular/material/button";
import { MatSelectModule } from "@angular/material/select";
import { NgSelectModule } from "@ng-select/ng-select";
import { AbonentsService } from "../../../abonents/services/abonents.service";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { NgxMaskDirective } from "ngx-mask";
import { RegisterUserRequest } from "../../models/RegisterUserRequest.model";

function generateRandomString(length) {
  const characters =
    '0123456789012345678901234567890123456789012345678901234567890123456789АБВГДСПР';
  const charactersLength = characters.length;
  let result = '';

  // Create an array of 32-bit unsigned integers
  const randomValues = new Uint32Array(length);

  // Generate random values
  window.crypto.getRandomValues(randomValues);
  randomValues.forEach((value) => {
    result += characters.charAt(value % charactersLength);
  });
  return result;
}

@Component({
  selector: 'app-create-abonent',
  templateUrl: './abonent-create.component.html',
  styleUrls: ['./abonent-create.component.scss'],
  standalone: true,
  imports: [CommonModule, MatFormFieldModule, MatInputModule, FormsModule, ReactiveFormsModule, MatButtonModule, MatSelectModule, NgSelectModule, NgxMaskDirective],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class AbonentCreateComponent {
  destroyRef = inject(DestroyRef)
  abonentsS = inject(AbonentsService)
  cdr = inject(ChangeDetectorRef)

  fb = inject(FormBuilder)
  loading$ = new BehaviorSubject(false)
  router = inject(Router)
  route = inject(ActivatedRoute)

  globalError = null

  form = this.fb.group({
    email: this.fb.control('', [Validators.required, Validators.email]),
    firstName: this.fb.control('', [Validators.required, Validators.minLength(1)]),
    secondName: this.fb.control('', [Validators.required, Validators.minLength(1)]),
    thirdName: this.fb.control('', [Validators.required, Validators.minLength(1)]),
  })

  create$() {
    console.error('create')
    if (this.form.invalid || this.loading$.value) {
      return
    }
    console.log(this.form.value)
    const {
      email,
      firstName,
      secondName,
      thirdName
    } = this.form.value
    const fio = [firstName, secondName, thirdName].join(' ')
    this.loading$.next(true)
    this.abonentsS.register$({
      email: email as string, fio
    })
      .subscribe({
        next: () => {
          this.loading$.next(false)
          this.router.navigate(['..'], { relativeTo: this.route })
        },
        error: (err) => {
          console.error(err)
          this.globalError = err.error
          // this.form = this.fb.group({
          //   email: this.fb.control(this.form.value.email, [Validators.required, Validators.email]),
          //   firstName: this.fb.control(this.form.value.firstName, [Validators.required, Validators.minLength(1)]),
          //   secondName: this.fb.control(this.form.value.secondName, [Validators.required, Validators.minLength(1)]),
          //   thirdName: this.fb.control(this.form.value.thirdName, [Validators.required, Validators.minLength(1)]),
          // }) as unknown as any
          this.loading$.next(false)
          this.cdr.detectChanges()
        },
        complete: () => {
          this.loading$.next(false)
          //this.form.reset(this.form.value)
        },
      })
  }

  ngAfterViewInit() {
    this.form.valueChanges.pipe(
      takeUntilDestroyed(this.destroyRef)
    ).subscribe(() => this.globalError = null)
  }
}
