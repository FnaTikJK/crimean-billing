import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import ListFilterComponent, { FilterField } from "../../../shared/components/list-filter/list-filter.component";
import { NgxDatatableModule, TableColumn } from '@siemens/ngx-datatable'
import BaseListWithFiltersComponent from '../../../shared/BaseListWithFilters.component';
import { MatButtonModule } from '@angular/material/button';
import { ManagersService } from './services/managers.service';

@Component({
  selector: 'app-managers',
  standalone: true,
  imports: [CommonModule, RouterModule, ListFilterComponent, NgxDatatableModule, MatButtonModule],
  templateUrl: './managers.component.html',
  styleUrl: './managers.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class ManagersComponent extends BaseListWithFiltersComponent {
  router = inject(Router)
  override service = inject(ManagersService)

  override filterFields: FilterField[] = [
    {
      field: 'fio',
      name: 'ФИО',
      type: 'text',
    },
  ]

  override columns: TableColumn[] = [
    {
      prop: 'id',
      name: 'ID',
      sortable: false,
    },
    {
      prop: 'fio',
      name: 'ФИО',
      sortable: false,
    },
  ]

  redirectOnItemPage(ev: any) {
    if (ev.type !== 'click') return
    this.router.navigate([`/main/managers/${ev.row.id}`])
  }
}
