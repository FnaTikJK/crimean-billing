import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatError, MatFormField, MatLabel, MatPrefix, MatSuffix } from '@angular/material/form-field';
import { MatIcon } from '@angular/material/icon';
import { MatButton, MatIconButton } from '@angular/material/button';
import { MatInput } from '@angular/material/input';
import { PasswordInputType } from '../../authorization/components/login.component';
import { MatDivider } from '@angular/material/divider';
import { ILoginManagerRequestDTO } from '../../authorization/DTO/requests/ILoginManagerRequestDTO';
import { tap } from 'rxjs';
import { AuthorizationService } from '../../authorization/services/authorization.service';
import { IChangeManagerPasswordRequest } from '../../authorization/DTO/requests/IChangeManagerPasswordRequest';
import { Router } from '@angular/router';
import { MyValidatorsService } from '@angular-monorepo/infrastructure';

@Component({
  selector: 'app-change-password',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatError, MatFormField, MatIcon, MatIconButton, MatInput, MatLabel, MatPrefix, MatSuffix, MatButton, MatDivider],
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class ChangePasswordComponent {

  #authorizationS = inject(AuthorizationService);
  #router = inject(Router);
  #myValidatorsS = inject(MyValidatorsService);

  protected changePasswordForm = new FormGroup({
    oldPassword: new FormControl<string>('', { validators: [Validators.required], nonNullable: true }),
    newPassword: new FormControl<string>('', { validators: [Validators.required], nonNullable: true }),
  }, { validators: [this.#myValidatorsS.shouldNotMatchValidator('oldPassword', 'newPassword')] });

  protected passwordInputTypeForOldPassword: PasswordInputType = 'password';
  protected passwordInputTypeForNewPassword: PasswordInputType = 'password';

  changePasswordVisibilityForOldPassword(ev: MouseEvent) {
    ev.stopPropagation();
    this.passwordInputTypeForOldPassword = this.passwordInputTypeForOldPassword === 'password' ? 'text' : 'password';
  }

  changePasswordVisibilityForNewPassword(ev: MouseEvent) {
    ev.stopPropagation();
    this.passwordInputTypeForNewPassword = this.passwordInputTypeForNewPassword === 'password' ? 'text' : 'password';
  }
  submitForm() {
    this.#authorizationS.changePassword$(this.changePasswordForm.value as IChangeManagerPasswordRequest)
      .subscribe(
        () => this.#router.navigate(['authorization'], { relativeTo: null }),
      );
  }
}
