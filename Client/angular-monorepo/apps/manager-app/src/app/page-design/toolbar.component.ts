import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatToolbar } from '@angular/material/toolbar';
import { MatMenu, MatMenuContent, MatMenuItem, MatMenuTrigger } from '@angular/material/menu';
import { MatIcon } from '@angular/material/icon';
import { MatFabButton } from '@angular/material/button';
import { MatDivider } from '@angular/material/divider';
import { AuthorizationService } from '../modules/authorization/services/authorization.service';
import { Router, RouterLink } from '@angular/router';
import { tap } from 'rxjs';

@Component({
  selector: 'app-toolbar',
  standalone: true,
  imports: [CommonModule, MatToolbar, MatMenu, MatIcon, MatMenuTrigger, MatMenuContent, MatMenuItem, MatFabButton, MatDivider, RouterLink],
  templateUrl: './toolbar.component.html',
  styleUrl: './toolbar.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ToolbarComponent {

  #authorizationS = inject(AuthorizationService);
  #router = inject(Router);
  protected unAuthorize() {
    this.#authorizationS.logOut$()
      .pipe(
        tap(() => this.#router.navigate(['authorization'], { relativeTo: null }))
      ).subscribe();
  }
}
