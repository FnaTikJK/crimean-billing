import { ChangeDetectionStrategy, Component, computed, inject, input, Signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  MatCard,
  MatCardContent,
  MatCardFooter,
  MatCardHeader,
  MatCardSubtitle,
  MatCardTitle
} from '@angular/material/card';
import { MatButton } from '@angular/material/button';
import { MatBottomSheet } from '@angular/material/bottom-sheet';
import { AddMoneyBottomSheetComponent } from '../add-money-bottom-sheet/add-money-bottom-sheet.component';
import { MatDivider } from '@angular/material/divider';
import { AccountService } from '../../../profile/submodules/account/services/account.service';
import { IAccount } from '../../../profile/submodules/account/models/IAccount';
import { AccountType } from '../../../profile/submodules/account/models/AccountType';

@Component({
  selector: 'app-account-card',
  standalone: true,
  imports: [CommonModule, MatCard, MatCardHeader, MatCardContent, MatCardFooter, MatCardTitle, MatCardSubtitle, MatButton, MatDivider],
  templateUrl: './account-card.component.html',
  styleUrl: './account-card.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AccountCardComponent {

  #accountS = inject(AccountService);
  #bottomSheet = inject(MatBottomSheet);

  accountID = input.required<string>();
  protected account: Signal<IAccount | undefined> = computed(() => this.#accountS.getID(this.accountID()));
  protected readonly AccountType = AccountType;

  protected openAddMoneyModal() {
    this.#bottomSheet.open(AddMoneyBottomSheetComponent, { data: { accountID: this.accountID() } });
  }
}

