import { Component, inject } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatIconButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { ToolbarComponent } from './page-design/toolbar.component';
import { AuthorizationService } from './modules/authorization/services/authorization.service';
@Component({
  standalone: true,
  imports: [RouterModule, MatIconButton, MatIcon, ToolbarComponent],
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {

  #authorizationS = inject(AuthorizationService);
  protected isAuthorized = this.#authorizationS.isAuthorized;
}
