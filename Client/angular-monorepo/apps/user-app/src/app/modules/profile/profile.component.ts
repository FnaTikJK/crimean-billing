import { ChangeDetectionStrategy, Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { ProfileService } from './services/profile.service';
import { MatButton } from '@angular/material/button';
import { AuthorizationService } from '../authorization/services/authorization.service';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, MatFormField, MatInput, ReactiveFormsModule, MatButton, MatLabel],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class ProfileComponent implements OnInit {

  #profileS = inject(ProfileService);
  #authS = inject(AuthorizationService);

  protected profileFormGroup = new FormGroup({
    fio: new FormControl(this.#profileS.profileState().entity?.fio),
    email: new FormControl(this.#profileS.profileState().entity?.email)
  });

  ngOnInit() {
    this.disableProfileFormGroup();
  }

  protected logout() {
    this.#authS.logOut$().subscribe();
  }

  private disableProfileFormGroup() {
    this.profileFormGroup.disable();
  }

}
