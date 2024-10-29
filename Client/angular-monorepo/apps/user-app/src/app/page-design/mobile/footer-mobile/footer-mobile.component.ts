import { ChangeDetectionStrategy, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { MatRipple } from '@angular/material/core';

@Component({
  selector: 'app-footer-mobile',
  standalone: true,
  imports: [CommonModule, MatIconButton, MatIcon, RouterLink, RouterLinkActive, MatRipple],
  templateUrl: './footer-mobile.component.html',
  styleUrl: './footer-mobile.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FooterMobileComponent {

}
