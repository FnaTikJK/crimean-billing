import { ChangeDetectionStrategy, Component, inject, OnInit, TemplateRef, viewChild } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import ListFilterComponent, { FilterField } from '../../../shared/components/list-filter/list-filter.component';
import { NgxDatatableModule, TableColumn } from '@siemens/ngx-datatable';
import BaseListWithFiltersComponent from '../../../shared/BaseListWithFilters.component';
import { PAYMENT_TYPES } from './models/PaymentType.enum';
import { IInvoice } from '../invoices/models/Invoice';
import { MatButton } from '@angular/material/button';
import { PaymentsService } from './services/payments.service';

@Component({
  selector: 'app-payments',
  standalone: true,
  imports: [CommonModule, RouterModule, ListFilterComponent, MatButton, NgxDatatableModule],
  templateUrl: './payments.component.html',
  styleUrl: './payments.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class PaymentsComponent extends BaseListWithFiltersComponent implements OnInit {
  router = inject(Router)

  dateTemplate = viewChild.required<TemplateRef<any>>('dateTemplate');

  override service = inject(PaymentsService)
  override filterFields: FilterField[] = [
    {
      field: 'paymentType',
      name: 'Тип операции',
      type: 'select',
      options: PAYMENT_TYPES
    }
  ]

  ngOnInit() {
    this.initColumns();
  }

  private initColumns() {
    this.columns = [
      {
        prop: 'money',
        name: 'Сумма'
      },
      {
        prop: 'type',
        name: 'Тип операции',
        pipe: { transform(val: number) { return PAYMENT_TYPES.find(({ value }) => value === val)?.name } }
      },
      {
        prop: 'invoice',
        name: 'Инвойс',
        pipe: { transform(invoice: IInvoice) { return invoice.id ?? '-' } }
      },
      {
        prop: 'dateTime',
        name: 'Время',
        cellTemplate: this.dateTemplate()
      },
    ];
  }

  redirectOnItemPage(ev: any) {
    if (ev.type !== 'click') return
    console.log(['services', ev.id])
    console.log(ev)
    this.router.navigate([`/main/payments/${ev.row.id}`])
  }
}
