import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatIcon } from '@angular/material/icon';
import { MatIconButton } from '@angular/material/button';
import { ToolbarComponent } from './page-design/desktop/toolbar/toolbar.component';
import { AuthorizationService } from './modules/authorization/services/authorization.service';
import { isMobile } from './modules/shared/help-functions';
import { FooterMobileComponent } from './page-design/mobile/footer-mobile/footer-mobile.component';
import { NgClass, NgStyle } from '@angular/common';

@Component({
  standalone: true,
  imports: [RouterModule, MatIcon, MatIconButton, ToolbarComponent, FooterMobileComponent, NgClass, NgStyle],
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AppComponent {

  #authS = inject(AuthorizationService);
  protected isAuthorized = this.#authS.isAuthorized;
  protected readonly isMobile = isMobile;
}
