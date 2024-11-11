import { ChangeDetectionStrategy, Component, computed, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatError, MatFormField, MatHint, MatLabel, MatPrefix } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ProfileService } from './services/profile.service';
import { MatButton, MatIconButton } from '@angular/material/button';
import { AuthorizationService } from '../authorization/services/authorization.service';
import { MatIcon } from '@angular/material/icon';
import { map, tap } from 'rxjs';
import { MatBottomSheet } from '@angular/material/bottom-sheet';
import { ModalConfirmComponent } from '../shared/modals/modal-confirm.component';
import { MatTooltip } from '@angular/material/tooltip';
import { IPatchUserRequestDTO } from './DTO/request/IPatchUserRequestDTO';
import { MatProgressSpinner } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, MatFormField, MatInput, ReactiveFormsModule, MatButton, MatLabel, MatIconButton, MatIcon, MatHint, MatError, MatTooltip, MatProgressSpinner, MatPrefix],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class ProfileComponent implements OnInit {

  #profileS = inject(ProfileService);
  #authS = inject(AuthorizationService);
  #matBottomSheet = inject(MatBottomSheet);

  protected currentMode = signal<ViewModeType>('view');
  protected saveBtnDisabled = signal(true);
  protected savedProfile = computed(() => ({
    fio: this.#profileS.profileState().entity?.fio,
    telegramId: this.#profileS.profileState().entity?.telegramId,
    email: this.#profileS.profileState().entity?.email
  }));

  protected profileFormGroup = new FormGroup({
    fio: new FormControl(this.#profileS.profileState().entity?.fio, { nonNullable: true, validators: [Validators.required, Validators.pattern(/^\s*[А-Яа-яёЕ]+\s[А-Яа-яёЕ]+(\s[А-Яа-яёЕd]+\s*)?$/)] }),
    telegramId: new FormControl(this.#profileS.profileState().entity?.telegramId, { validators: [Validators.pattern(/\d+$/)] }),
    email: new FormControl(this.#profileS.profileState().entity?.email, { nonNullable: true, validators: [Validators.required, Validators.email] }),
  });

  protected loading = signal(false);

  ngOnInit() {
    this.disableProfileFormGroup();
    this.listenFormChange();
  }

  protected changeMode(newMode: ViewModeType) {
    if (newMode === 'view') {
      if (this.formHasChanged()) {
        this.#matBottomSheet.open(ModalConfirmComponent,
          { data: 'Вы уверены, что хотите выйти из режима редактирования? Все несохранённые изменения будут утеряны'
          })
          .afterDismissed()
          .subscribe(result => {
            if (result) {
              this.currentMode.set('view');
              this.setFormValueToSaved();
              this.profileFormGroup.disable({emitEvent: false});
            }
          })
      }
      else {
        this.currentMode.set('view');
        this.setFormValueToSaved();
        this.profileFormGroup.disable({emitEvent: false});
      }
    } else {
      this.profileFormGroup.enable({emitEvent: false});
      this.currentMode.set('edit');
    }
  }

  protected submitChanges() {
    this.loading.set(true)
    this.#profileS.patchProfile$(this.mapFormValue() as IPatchUserRequestDTO)
      .pipe(
        tap(() => {
          this.changeMode('view');
          this.loading.set(false);
        })
      )
      .subscribe();
  }

  protected logout() {
    this.#authS.logOut$().subscribe();
  }

  private disableProfileFormGroup() {
    this.profileFormGroup.disable();
  }

  private setFormValueToSaved() {
    this.profileFormGroup.setValue(this.savedProfile());
  }

  private mapFormValue() {
    const formValue = this.profileFormGroup.value;
    return  {
      ...formValue,
      fio: formValue.fio ? formValue.fio.trim() : '',
      telegramId: formValue.telegramId ? +formValue.telegramId : null
    };
  }

  private formHasChanged() {
    const valueMapped = this.mapFormValue();
    return JSON.stringify(valueMapped) !== JSON.stringify(this.savedProfile());
  }

  private listenFormChange() {
    this.profileFormGroup.valueChanges
      .pipe(
        map((value) =>  this.formHasChanged())
      ).subscribe(valueChanged => this.saveBtnDisabled.set(!valueChanged || this.profileFormGroup.invalid));
  }

}

type ViewModeType = 'view' | 'edit';
