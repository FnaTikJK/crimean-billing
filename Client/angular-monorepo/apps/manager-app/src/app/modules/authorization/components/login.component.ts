import { ChangeDetectionStrategy, Component, DestroyRef, effect, inject, OnInit } from '@angular/core';
import { CommonModule, NgOptimizedImage } from '@angular/common';
import { MatError, MatFormField, MatHint, MatLabel, MatPrefix, MatSuffix } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { MatCheckbox } from '@angular/material/checkbox';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton, MatIconButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { AuthorizationService } from '../services/authorization.service';
import { ILoginManagerRequestDTO } from '../DTO/requests/ILoginManagerRequestDTO';
import { Router } from '@angular/router';
import { ManagerSettingsService } from '../../shared/services/manager-settings.service';
import { tap } from 'rxjs';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, MatFormField, MatInput, MatHint, MatLabel,
    NgOptimizedImage, MatCheckbox, ReactiveFormsModule, MatButton,
    MatIcon, MatSuffix, MatPrefix, MatIconButton,
    MatError
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class LoginComponent {

  private authorizationS = inject(AuthorizationService);
  private managerSettingsS = inject(ManagerSettingsService);
  private router = inject(Router);

  protected authorizationForm = new FormGroup({
    login: new FormControl<string>('', { validators: [Validators.required], nonNullable: true }),
    password: new FormControl<string>('', { validators: [Validators.required], nonNullable: true })
  });

  protected saveCredentialsControl = new FormControl<boolean>(true, {nonNullable: true});

  protected passwordInputType: PasswordInputType = 'password';

  protected changePasswordVisibility(ev: MouseEvent) {
    ev.stopPropagation();
    this.passwordInputType = this.passwordInputType === 'password' ? 'text' : 'password';
  }

  submitForm() {
    this.authorizationS.login$(this.authorizationForm.value as ILoginManagerRequestDTO)
      .pipe(
        tap(() => this.managerSettingsS.updateSettings({ saveCredentials: this.saveCredentialsControl.value }))
      )
      .subscribe(res => this.router.navigate(['main'], { relativeTo: null }));
  }
}

export type PasswordInputType = 'text' | 'password';
