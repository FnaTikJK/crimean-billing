import { Component, inject } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatIconButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { ToolbarComponent } from './page-design/toolbar.component';
import { AuthorizationService } from './modules/authorization/services/authorization.service';
import { MatSidenavModule } from '@angular/material/sidenav';
@Component({
  standalone: true,
  imports: [RouterModule, MatIconButton, MatIcon, ToolbarComponent, MatSidenavModule],
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {

  #authorizationS = inject(AuthorizationService);
  protected isAuthorized = this.#authorizationS.isAuthorized;
}
