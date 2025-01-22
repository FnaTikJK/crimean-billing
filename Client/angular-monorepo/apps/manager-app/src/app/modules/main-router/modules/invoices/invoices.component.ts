import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import ListFilterComponent, { FilterField } from "../../../shared/components/list-filter/list-filter.component";
import { NgxDatatableModule, TableColumn } from '@siemens/ngx-datatable'
import BaseListWithFiltersComponent from '../../../shared/BaseListWithFilters.component';
import { MatButtonModule } from '@angular/material/button';
import { InvoicesService } from './services/invoices.service';

@Component({
  selector: 'app-invoices',
  standalone: true,
  imports: [CommonModule, RouterModule, ListFilterComponent, NgxDatatableModule, MatButtonModule],
  templateUrl: './invoices.component.html',
  styleUrl: './invoices.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class ServicesComponent extends BaseListWithFiltersComponent {
  router = inject(Router)
  override service = inject(InvoicesService)

  override filterFields: FilterField[] = [
    {
      field: 'accountId',
      name: 'ID ЛС',
      type: 'text',
    },
    {
      field: 'toPay',
      name: 'Сумма',
      type: 'number-range',
    },
    {
      field: 'createdAt',
      name: 'Дата создания',
      type: 'dateRange',
    },
    {
      field: 'payedAt',
      name: 'Дата оплачено',
      type: 'dateRange',
    },
    {
      field: 'money',
      name: 'Сумма',
      type: 'number-range',
    },
  ]

  override columns: TableColumn[] = [
    {
      prop: 'toPay',
      name: 'Сумма',
      sortable: true,
    },
    {
      prop: 'accountId',
      name: 'ЛС',
      sortable: false,
    },
    {
      prop: 'payedAt',
      name: 'Дата оплаты',
      sortable: true,
      pipe: { transform(val: any) { return val ? val : '-' } }
    },
    {
      prop: 'createdAt',
      name: 'Дата создания',
      sortable: true,
      pipe: { transform(val: any) { return val ? val : '-' } }
    },
  ]

  redirectOnItemPage(ev: any) {
    if (ev.type !== 'click') return
    this.router.navigate([`/main/invoices/${ev.row.id}`])
  }
}
