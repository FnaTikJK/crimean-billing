import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NxWelcomeComponent } from './nx-welcome.component';
import { MatIconButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
@Component({
  standalone: true,
  imports: [NxWelcomeComponent, RouterModule, MatIconButton, MatIcon],
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  title = 'manager-app';
  constructor() {
    console.log('ManagerApp started')
  }
}
