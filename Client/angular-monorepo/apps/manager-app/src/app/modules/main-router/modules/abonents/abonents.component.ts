import { ChangeDetectionStrategy, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import ListFilterComponent, { FilterField } from '../../../shared/components/list-filter/list-filter.component';

@Component({
  selector: 'app-abonents',
  standalone: true,
  imports: [CommonModule, RouterModule, ListFilterComponent],
  templateUrl: './abonents.component.html',
  styleUrl: './abonents.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class AbonentsComponent {
  listFilterConfig: FilterField[] = [
    { field: 'ff', name: 'name', type: 'text' },
    { field: 'ff2', name: 'name2', type: 'number' },
    { field: 'ff3', name: 'name3', type: 'date' },
    { field: 'ff4', name: 'name4', type: 'dateRange' },
    { field: 'ff5', name: 'name5', type: 'select', options: [ {name:'asd',value:'asd2'}] },
  ]

  log(...args: any[]) {
    console.error('filterChange', ...args)
  }
}
