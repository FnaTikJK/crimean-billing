import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { ServicesService } from '../services/services/services.service';
import { FilterField } from '../../../shared/components/list-filter/list-filter.component';
import { ACCOUNT_TYPES } from '../accounts/models/AccountType.enum';
import { SERVICE_TYPES } from '../services/models/ServiceType.enum';
import { UNIT_TYPES } from '../services/models/UnitType.enum';
import { TableColumn } from '@siemens/ngx-datatable';
import { SearchServiceRequestOrderBy } from '../services/models/DTO/SearchServiceRequestOrderBy.model';
import BaseListWithFiltersComponent from '../../../shared/BaseListWithFilters.component';
import { PAYMENT_TYPES } from './models/PaymentType.enum';
import { IInvoice } from './models/Invoice';

@Component({
  selector: 'app-payments',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './payments.component.html',
  styleUrl: './payments.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class PaymentsComponent extends BaseListWithFiltersComponent {
  router = inject(Router)
  override service = inject(ServicesService)
  override filterFields: FilterField[] = [
    {
      field: 'name',
      name: 'Абонент',
      type: 'select',
      options: PAYMENT_TYPES,
    },
    {
      field: 'paymentType',
      name: 'Тип операции',
      type: 'select',
      options: PAYMENT_TYPES,
      initialValue: undefined
    }
  ]

  override columns: TableColumn[] = [
    {
      prop: 'dateTime',
      name: 'Время',
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
    }
  ]

  override fieldToOrder = {
    'amount': SearchServiceRequestOrderBy.Amount,
    'price': SearchServiceRequestOrderBy.Price,
    'code': SearchServiceRequestOrderBy.Code,
  }

  redirectOnItemPage(ev: any) {
    if (ev.type !== 'click') return
    console.log(['services', ev.id])
    console.log(ev)
    this.router.navigate([`/main/services/${ev.row.id}`])
  }
}
