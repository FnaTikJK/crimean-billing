import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MAT_BOTTOM_SHEET_DATA, MatBottomSheetRef } from '@angular/material/bottom-sheet';

@Component({
  selector: 'app-confirm',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './modal-confirm.component.html',
  styleUrl: './modal-confirm.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ModalConfirmComponent {

  #matBottomSheetRef = inject(MatBottomSheetRef);
  protected confirmMsg: string = inject(MAT_BOTTOM_SHEET_DATA);

  protected close(result: boolean) {
    this.#matBottomSheetRef.dismiss(result);
  }

}
