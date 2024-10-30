import {
  ChangeDetectionStrategy,
  Component,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTab, MatTabContent, MatTabGroup } from '@angular/material/tabs';
import { MatCard } from '@angular/material/card';
import { MatIcon } from '@angular/material/icon';
import { MatSlideToggle } from '@angular/material/slide-toggle';
import { InvoicesListComponent } from './submodules/invoices/components/invoices-list/invoices-list.component';

@Component({
  selector: 'app-expenses',
  standalone: true,
  imports: [CommonModule, MatTabGroup, MatTab, MatTabContent, MatCard, MatIcon, MatSlideToggle, InvoicesListComponent],
  templateUrl: './expenses.component.html',
  styleUrl: './expenses.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class ExpensesComponent  {
}

