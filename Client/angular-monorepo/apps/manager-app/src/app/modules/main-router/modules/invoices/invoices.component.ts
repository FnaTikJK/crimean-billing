import { ChangeDetectionStrategy, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-invoices',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './invoices.component.html',
  styleUrl: './invoices.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class AInvoicesComponent { }
