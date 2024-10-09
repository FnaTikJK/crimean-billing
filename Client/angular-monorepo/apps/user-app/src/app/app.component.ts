import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NxWelcomeComponent } from './nx-welcome.component';
import { MatIcon } from '@angular/material/icon';
import { MatIconButton } from '@angular/material/button';
@Component({
  standalone: true,
  imports: [NxWelcomeComponent, RouterModule, MatIcon, MatIconButton],
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  title = 'user-app';
  constructor() {
    console.log('UserApp started')
  }
}
