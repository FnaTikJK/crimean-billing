import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import ListFilterComponent, { FilterField } from "../../../shared/components/list-filter/list-filter.component";
import { TariffsService } from './services/tariffs.service'
import { ACCOUNT_TYPES } from '../accounts/models/AccountType.enum';
import { NgxDatatableModule, TableColumn } from '@siemens/ngx-datatable'
import BaseListWithFiltersComponent from '../../../shared/BaseListWithFilters.component';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-tariffs',
  standalone: true,
  imports: [CommonModule, RouterModule, ListFilterComponent, NgxDatatableModule, MatButtonModule],
  templateUrl: './tariffs.component.html',
  styleUrl: './tariffs.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class TariffsComponent extends BaseListWithFiltersComponent {
  router = inject(Router)
  override service = inject(TariffsService)
  override filterFields: FilterField[] = [
    {
      field: 'name',
      name: 'Название',
      type: 'text',
    },
    {
      field: 'code',
      name: 'Код',
      type: 'text',
    },
    {
      field: 'accountType',
      name: 'Тип ЛС',
      type: 'select',
      options: ACCOUNT_TYPES,
    },
    {
      field: 'price',
      name: 'Цена',
      type: 'number-range',
    },
  ]

  override columns: TableColumn[] = [
    {
      prop: 'code',
      name: 'Код',
      sortable: false,
    },
    {
      prop: 'name',
      name: 'Название',
      sortable: false,
    },
    {
      prop: 'accountType',
      name: 'Тип ЛС',
      sortable: false,
      pipe: { transform(val: any) { return ACCOUNT_TYPES.find(({ value }) => value === val)?.name } }
    },
    {
      prop: 'price',
      name: 'Цена',
      sortable: false,
    }
  ]

  redirectOnItemPage(ev: any) {
    if (ev.type !== 'click') return
    console.log(['services', ev.id])
    console.log(ev)
    this.router.navigate([`/main/services/${ev.row.id}`])
  }
}
