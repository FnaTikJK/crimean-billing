import { ChangeDetectionStrategy, Component, inject } from "@angular/core";
import { ManagersService } from "../../services/managers.service";
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { BehaviorSubject, of } from "rxjs";
import { CommonModule } from "@angular/common";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { ActivatedRoute, Router } from "@angular/router";
import { MatButtonModule } from "@angular/material/button";

@Component({
  selector: 'app-create-manager',
  templateUrl: './managers-create.component.html',
  styleUrls: ['./managers-create.component.scss'],
  standalone: true,
  imports: [CommonModule, MatFormFieldModule, MatInputModule, FormsModule, ReactiveFormsModule, MatButtonModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class ManagersCreateComponent {
  managersS = inject(ManagersService)
  fb = inject(FormBuilder)
  loading$ = new BehaviorSubject(false)
  router = inject(Router)
  route = inject(ActivatedRoute)

  form = this.fb.group({
    login: this.fb.control('', [Validators.required, Validators.minLength(1)]),
    firstName: this.fb.control('', [Validators.required, Validators.minLength(1)]),
    secondName: this.fb.control('', [Validators.required, Validators.minLength(1)]),
    thirdName: this.fb.control('', [Validators.required, Validators.minLength(1)]),
    password: this.fb.control('', [Validators.required, Validators.minLength(5)]),
  })

  controls = [
    {
      translation: 'Логин',
      control: this.form.controls.login,
    },
    {
      translation: 'Имя',
      control: this.form.controls.firstName,
    },
    {
      translation: 'Фамилия',
      control: this.form.controls.secondName,
    },
    {
      translation: 'Отчество',
      control: this.form.controls.thirdName,
    },
    {
      translation: 'Пароль',
      control: this.form.controls.password,
      inputType: 'password',
    },
  ]

  create$() {
    if (this.form.invalid || this.loading$.value) {
      return
    }
    const {
      login,
      firstName,
      secondName,
      thirdName,
      password
    } = this.form.value
    const fio = [firstName, secondName, thirdName].join(' ')
    this.loading$.next(true)
    this.managersS.register$({ login: login!, password: password!, fio })
    .subscribe(() => {
      this.loading$.next(false)
      this.router.navigate(['..'], { relativeTo: this.route })
    })
  }
}
