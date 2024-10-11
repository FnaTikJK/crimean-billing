import { inject, Injectable, NgZone } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class CustomErrorHandlerService {

  private snackBar = inject(MatSnackBar);
  private ngZone = inject(NgZone);

  handleError(error: Error & { error?: string; status?: number }): void {
    this.snackBar.dismiss();
    this.ngZone.run(() => {
      this.snackBar.open(error['error'] || error.message, 'Закрыть', {
        duration: 1500,
      });
    });
  }
}
