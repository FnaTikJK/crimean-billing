import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatError, MatFormField, MatLabel, MatSuffix } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { FormControl, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MAT_BOTTOM_SHEET_DATA, MatBottomSheetRef } from '@angular/material/bottom-sheet';
import { AccountService } from '../../../profile/submodules/account/services/account.service';
import { tap } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-add-money-bottom-sheet',
  standalone: true,
  imports: [CommonModule, MatFormField, MatInput, ReactiveFormsModule, MatLabel, MatButton, MatSuffix, MatError, FormsModule],
  templateUrl: './add-money-bottom-sheet.component.html',
  styleUrl: './add-money-bottom-sheet.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AddMoneyBottomSheetComponent {

  #accountS = inject(AccountService);
  #matSnackbar = inject(MatSnackBar);
  #matBottomSheetRef = inject(MatBottomSheetRef);

  #accountID: string =  inject(MAT_BOTTOM_SHEET_DATA);

  protected addMoneyControl = new FormControl<number>(1, {
    validators: [Validators.min(1), Validators.max(1000000)],
    nonNullable: true
  });

  protected submitForm() {
    this.#accountS.addMoney$({ accountId: this.#accountID, toAdd: this.addMoneyControl.value })
      .pipe(
        tap(() => {
          this.#matBottomSheetRef.dismiss();
          this.#matSnackbar.open('Счёт успешно полнен', 'Закрыть', { duration: 1500 });
        })
      ).subscribe()
  }
}
