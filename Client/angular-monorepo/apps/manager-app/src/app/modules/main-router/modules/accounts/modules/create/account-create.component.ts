import { ChangeDetectionStrategy, Component, DestroyRef, inject } from "@angular/core";
import { AccountsService } from "../../services/accounts.service";
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { BehaviorSubject, combineLatest, distinctUntilChanged, forkJoin, map, of, switchMap, take, tap } from "rxjs";
import { CommonModule } from "@angular/common";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { ActivatedRoute, Router } from "@angular/router";
import { MatButtonModule } from "@angular/material/button";
import { MatSelectModule } from "@angular/material/select";
import { ACCOUNT_TYPES } from "../../models/AccountType.enum";
import { NgSelectModule } from "@ng-select/ng-select";
import { AbonentsService } from "../../../abonents/services/abonents.service";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { RegisterAccountRequest } from "../../models/DTO/RegisterAccountRequest.model";
import { NgxMaskDirective } from "ngx-mask";

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
  selector: 'app-create-account',
  templateUrl: './account-create.component.html',
  styleUrls: ['./account-create.component.scss'],
  standalone: true,
  imports: [CommonModule, MatFormFieldModule, MatInputModule, FormsModule, ReactiveFormsModule, MatButtonModule, MatSelectModule, NgSelectModule, NgxMaskDirective],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class ManagersCreateComponent {
  destroyRef = inject(DestroyRef)

  accountsS = inject(AccountsService)
  abonentsS = inject(AbonentsService)

  fb = inject(FormBuilder)
  loading$ = new BehaviorSubject(false)
  router = inject(Router)
  route = inject(ActivatedRoute)

  ACCOUNT_TYPES = ACCOUNT_TYPES

  form = this.fb.group({
    accountType: this.fb.control('', [Validators.required]),
    phoneNumber: this.fb.control('', [Validators.required, Validators.minLength(1)]),
    userId: this.fb.control('', [Validators.required]),
    number: this.fb.control(generateRandomString(20), [Validators.required, Validators.minLength(1)]),
  })

  pagination$ = new BehaviorSubject({ skip: 0, take: 100 })
  abonentsInput$ = new BehaviorSubject<string>('');
  abonents$ = new BehaviorSubject([])

  create$() {
    if (this.form.invalid || this.loading$.value) {
      return
    }
    console.log(this.form.value)
    const {
      accountType,
      number,
      phoneNumber,
      userId,
    } = this.form.value as unknown as RegisterAccountRequest
    // const fio = [firstName, secondName, thirdName].join(' ')
    this.loading$.next(true)
    this.accountsS.register$({
      accountType,
      number,
      phoneNumber: '8' + phoneNumber,
      userId,
     })
    .subscribe(() => {
      this.loading$.next(false)
      this.router.navigate(['..'], { relativeTo: this.route })
    })
  }

  makeRequest$() {
    return combineLatest([
      this.pagination$,
      this.abonentsInput$,
    ])
      .pipe(
        tap(console.log),
        switchMap(([{ skip, take }, input]) => {
          const options: { fio?: string } = {}
          if (input && input.length) {
            options.fio = input
          }
          return this.abonentsS.search$(skip, take, options)
        }),
        map(results => results.items),
        take(1)
      )
  }

  onScroll(ev: any) {
    const value = this.pagination$.value
    this.pagination$.next({ ...value, take: value.take + 100 })
    this.makeRequest$().subscribe(abonents => this.abonents$.next(this.abonents$.value.concat(abonents)))
  }

  ngOnInit() {
    this.abonentsInput$
    .pipe(
      takeUntilDestroyed(this.destroyRef),
      distinctUntilChanged(),
    ).subscribe(() => {
      this.pagination$.next({ skip: 0, take: 100 })
    })
    this.makeRequest$().subscribe(abonents => this.abonents$.next(abonents))
  }
}
