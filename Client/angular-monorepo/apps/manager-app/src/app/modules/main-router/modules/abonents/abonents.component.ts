import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import ListFilterComponent, { FilterField } from "../../../shared/components/list-filter/list-filter.component";

import { NgxDatatableModule, TableColumn } from '@siemens/ngx-datatable'
import BaseListWithFiltersComponent from '../../../shared/BaseListWithFilters.component';
import { MatButtonModule } from '@angular/material/button';
import { AbonentsService } from './services/abonents.service';

@Component({
  selector: 'app-abonents',
  standalone: true,
  imports: [CommonModule, RouterModule, ListFilterComponent, NgxDatatableModule, MatButtonModule],
  templateUrl: './abonents.component.html',
  styleUrl: './abonents.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class ServicesComponent extends BaseListWithFiltersComponent {
  router = inject(Router)
  override service = inject(AbonentsService)

  override filterFields: FilterField[] = [
    {
      field: 'phoneNumber',
      name: 'Номер телефона',
      type: 'text',
    },
    {
      field: 'email',
      name: 'Email',
      type: 'text',
    },
    {
      field: 'fio',
      name: 'ФИО',
      type: 'text',
    },
  ]

  override columns: TableColumn[] = [
    {
      prop: 'fio',
      name: 'ФИО',
      sortable: false,
    },
    {
      prop: 'email',
      name: 'Email',
      sortable: false,
    },
    {
      prop: 'phoneNumber',
      name: 'Номер телефона',
      sortable: false,
    },
  ]

  redirectOnItemPage(ev: any) {
    if (ev.type !== 'click') return
    this.router.navigate([`/main/abonents/${ev.row.userId}`])
  }
}
