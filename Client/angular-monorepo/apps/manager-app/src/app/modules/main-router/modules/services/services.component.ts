import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import ListFilterComponent, { FilterField } from "../../../shared/components/list-filter/list-filter.component";
import { ServicesService } from './services/services.service'
import { ACCOUNT_TYPES } from '../accounts/models/AccountType.enum';
import { SERVICE_TYPES } from './models/ServiceType.enum';
import { UNIT_TYPES } from './models/UnitType.enum';
import { NgxDatatableModule, TableColumn } from '@siemens/ngx-datatable'
import BaseListWithFiltersComponent from '../../../shared/BaseListWithFilters.component';
import { SearchServiceRequestOrderBy } from './models/DTO/SearchServiceRequestOrderBy.model';

@Component({
  selector: 'app-services',
  standalone: true,
  imports: [CommonModule, RouterModule, ListFilterComponent, NgxDatatableModule],
  templateUrl: './services.component.html',
  styleUrl: './services.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class ServicesComponent extends BaseListWithFiltersComponent {
  router = inject(Router)
  override service = inject(ServicesService)
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
      field: 'serviceType',
      name: 'Тип услуги',
      type: 'select',
      options: SERVICE_TYPES,

    },
    {
      field: 'unitType',
      name: 'Единицы измерения',
      type: 'select',
      options: UNIT_TYPES,

    },
    {
      field: 'price',
      name: 'Цена',
      type: 'number-range',
    },
    {
      field: 'amount',
      name: 'Количество',
      type: 'number-range',
    }
  ]

  override columns: TableColumn[] = [
    {
      prop: 'code',
      name: 'Код',
      sortable: true,
    },
    {
      prop: 'name',
      name: 'Название',
      sortable: true,
    },
    {
      prop: 'accountType',
      name: 'Тип ЛС',
      sortable: false,
      pipe: { transform(val: any) { return ACCOUNT_TYPES.find(({ value }) => value === val)?.name } }
    },
    {
      prop: 'serviceType',
      name: 'Тип услуги',
      sortable: false,
      pipe: { transform(val: any) { return SERVICE_TYPES.find(({ value }) => value === val)?.name } }
    },
    {
      prop: 'unitType',
      name: 'Измерение',
      sortable: false,
      pipe: {transform(val: any){ return UNIT_TYPES.find(({value}) => value === val)?.name  }}
    },
    {
      prop: 'amount',
      name: 'Кол-во',
      sortable: true,
    },
    {
      prop: 'price',
      name: 'Цена',
      sortable: false,
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
