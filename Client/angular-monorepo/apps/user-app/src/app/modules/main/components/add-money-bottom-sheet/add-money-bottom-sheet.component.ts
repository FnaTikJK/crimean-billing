import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatError, MatFormField, MatLabel, MatSuffix } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { FormControl, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MAT_BOTTOM_SHEET_DATA, MatBottomSheetRef } from '@angular/material/bottom-sheet';
import { tap } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';
import { PaymentsService } from '../../../expenses/submodules/payments/services/payments.service';

@Component({
  selector: 'app-add-money-bottom-sheet',
  standalone: true,
  imports: [CommonModule, MatFormField, MatInput, ReactiveFormsModule, MatLabel, MatButton, MatSuffix, MatError, FormsModule],
  templateUrl: './add-money-bottom-sheet.component.html',
  styleUrl: './add-money-bottom-sheet.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AddMoneyBottomSheetComponent {

  #paymentS = inject(PaymentsService);
  #matSnackbar = inject(MatSnackBar);
  #matBottomSheetRef = inject(MatBottomSheetRef);

  #config: IAddMoneyBottomSheetConfig =  inject(MAT_BOTTOM_SHEET_DATA);

  protected addMoneyControl = new FormControl<number>(this.#config.defaultMoneyValue ?? 1, {
    validators: [Validators.min(1), Validators.max(1000000)],
    nonNullable: true
  });

  protected submitForm() {
    this.#paymentS.addMoney$({ accountId: this.#config.accountID, toAdd: this.addMoneyControl.value })
      .pipe(
        tap(() => {
          this.#matBottomSheetRef.dismiss(true);
          if (!this.#config.showSnackbar) { return; }
          this.#matSnackbar.open('Счёт успешно полнен', 'Закрыть', { duration: 1500 });
        }),
      ).subscribe()
  }
}

export interface IAddMoneyBottomSheetConfig {
  accountID: string;
  defaultMoneyValue?: number;
  showSnackbar?: boolean;
}
