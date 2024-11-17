import { ChangeDetectionStrategy, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-managers',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './managers.component.html',
  styleUrl: './managers.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class ManagersComponent { }
