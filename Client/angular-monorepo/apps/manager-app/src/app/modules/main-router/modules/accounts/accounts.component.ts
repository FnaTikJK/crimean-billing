import { ChangeDetectionStrategy, Component, inject, viewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import ListFilterComponent, { FilterField } from "../../../shared/components/list-filter/list-filter.component";
import { ACCOUNT_TYPES } from '../accounts/models/AccountType.enum';
import { NgxDatatableModule, TableColumn } from '@siemens/ngx-datatable'
import BaseListWithFiltersComponent from '../../../shared/BaseListWithFilters.component';
import { MatButtonModule } from '@angular/material/button';
import { AccountsService } from './services/accounts.service';

@Component({
  selector: 'app-accounts',
  standalone: true,
  imports: [CommonModule, RouterModule, ListFilterComponent, NgxDatatableModule, MatButtonModule],
  templateUrl: './accounts.component.html',
  styleUrl: './accounts.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class ServicesComponent extends BaseListWithFiltersComponent {
  abonentsLinkTemplate = viewChild('accountsLinkTemplate')
  router = inject(Router)
  override service = inject(AccountsService)

  override filterFields: FilterField[] = [
    {
      field: 'phoneNumber',
      name: 'Номер телефона',
      type: 'text',
    },
    {
      field: 'number',
      name: 'ЛС',
      type: 'text',
    },
    {
      field: 'accountType',
      name: 'Тип ЛС',
      type: 'select',
      options: ACCOUNT_TYPES,
    },
    {
      field: 'money',
      name: 'Сумма',
      type: 'number-range',
    },
  ]

  override columns: TableColumn[] = []

  redirectOnItemPage(ev: any) {
    if (ev.type !== 'click') return
    this.router.navigate([`/main/accounts/${ev.row.id}`])
  }

  navigate(ev: any, item: any) {
    console.error(item)
    ev.preventDefault()
    ev.stopPropagation()
    this.router.navigate(['main', 'abonents', item?.user?.userId])
  }

  override ngAfterViewInit(): void {
    this.columns = [
      {
        prop: 'number',
        name: 'ЛС',
        sortable: false,
      },
      {
        prop: 'user',
        name: 'ФИО',
        sortable: false,
        cellTemplate: this.abonentsLinkTemplate()
      },
      {
        prop: 'accountType',
        name: 'Тип ЛС',
        sortable: false,
        pipe: { transform(val: any) { return ACCOUNT_TYPES.find(({ value }) => value === val)?.name } }
      },
      {
        prop: 'money',
        name: 'Сумма',
        sortable: true,
      },
    ]
    super.ngAfterViewInit()
  }
}
