import {
  ChangeDetectionStrategy,
  Component,
  computed,
  DestroyRef,
  effect,
  inject,
  Injector,
  OnInit, Signal
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppSettingsService } from '../../../modules/shared/services/app-settings.service';
import { MatToolbar } from '@angular/material/toolbar';
import { MatFormField, MatLabel, MatOption, MatSelect } from '@angular/material/select';
import { skip } from 'rxjs';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatIconButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatMenu, MatMenuContent, MatMenuItem, MatMenuTrigger } from '@angular/material/menu';
import { isMobile } from '../../../modules/shared/help-functions';
import { RouterLink } from '@angular/router';
import { TitleService } from '../../services/title.service';
import { AccountService } from '../../../modules/profile/submodules/account/services/account.service';
import { IAccount } from '../../../modules/profile/submodules/account/models/IAccount';

@Component({
  selector: 'app-toolbar',
  standalone: true,
  imports: [CommonModule, MatToolbar, MatSelect, MatLabel, MatOption, MatFormField, ReactiveFormsModule, MatIconButton, MatIcon, MatMenuTrigger, MatMenu, MatMenuContent, MatMenuItem, RouterLink],
  templateUrl: './toolbar.component.html',
  styleUrl: './toolbar.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ToolbarComponent implements OnInit {

  #appSettingsS = inject(AppSettingsService);
  #accountS = inject(AccountService);
  #titleS = inject(TitleService);
  #injector = inject(Injector);
  #destroyRef = inject(DestroyRef);

  protected accounts: Signal<IAccount[] | null> = computed(() => this.#accountS.accountsState().entities);
  protected selectedAccountControl = new FormControl<string>('', { nonNullable: true });
  protected title = this.#titleS.title;

  ngOnInit() {
    this.listenForAppSettingsInit();
    this.listenForSelectedAccountChange();
  }

  private listenForAppSettingsInit() {
    const appSettingsEffect = effect(() => {
      const appSettings = this.#appSettingsS.appSettingsState();
      if (appSettings.checked) {
        const selectedAccountID = appSettings.entity!.accountSelected!;
        this.selectedAccountControl.setValue(selectedAccountID);
        appSettingsEffect.destroy();
      }
    }, { injector: this.#injector });
  }

  private listenForSelectedAccountChange() {
    this.selectedAccountControl.valueChanges
      .pipe(
        skip(1),
        takeUntilDestroyed(this.#destroyRef)
      ).subscribe((accountID) => this.#appSettingsS.selectAccount(accountID))
  }

  protected readonly isMobile = isMobile;
}
