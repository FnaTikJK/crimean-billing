import { ChangeDetectionStrategy, Component, ElementRef, EventEmitter, Input, OnChanges, Output, SimpleChanges, viewChild, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav'
import { MatToolbarModule } from '@angular/material/toolbar'
import { MatButtonModule } from '@angular/material/button'
import { MatSelectModule } from '@angular/material/select'
import { MatFormFieldModule } from '@angular/material/form-field'
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { MatGridListModule } from '@angular/material/grid-list';
import { OwlDateTimeModule, OwlNativeDateTimeModule } from '@danielmoncada/angular-datetime-picker'
import { MatMenuModule } from '@angular/material/menu';
import set from 'lodash/set'
import cloneDeep from 'lodash/cloneDeep';

export type ListFilterType = 'text' | 'number' | 'date' | 'dateRange' | 'select' | 'number-range'

export interface FilterParams {
  [key: string]: any;
}

export interface FilterField {
  field: string | number;
  name: string;
  type: ListFilterType;
  options?: { name: string, value: any }[];
  initialValue?: any;
}

@Component({
  selector: 'app-list-filter',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatSidenavModule,
    MatToolbarModule,
    MatButtonModule,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatDatepickerModule,
    MatInputModule,
    MatSelectModule,
    OwlDateTimeModule,
    OwlNativeDateTimeModule,
    MatGridListModule,
    MatMenuModule,
  ],
  templateUrl: './list-filter.component.html',
  styleUrl: './list-filter.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class ListFilterComponent implements OnChanges {
  @ViewChild('container') container!: ElementRef<HTMLDivElement>

  @Input() filterFields: FilterField[] = [];
  @Output() filterChange = new EventEmitter<FilterParams>();

  filters: FilterParams = {};

  ngOnChanges(changes: SimpleChanges): void {
    const filterFields: FilterField[] = changes?.['filterFields']?.currentValue
    if (Array.isArray(filterFields)) {
      this.filters = {}
      for (const field of filterFields) {
        if (field.type === 'number-range') {
          this.filters[field.field] = { from: undefined, to: undefined }
        } else {
        this.filters[field.field] = field.initialValue
        }
      }
    }
    console.log('filters', this.filters)
  }

  onSelectChange(field: any, value: any) {
    console.log('onSelectChange', field, value)
    this.onFilterChange(field, value.value)
  }

  onInputChange(field: any, value: any, isNumber = false) {
    console.log(field ,value)
    value = value.target.value
    if (value === '') {
      value = undefined
    }
    if (isNumber && typeof value === 'string') {
      value = +value
    }
    this.onFilterChange(field, value)
  }

  onFilterChange(field: string | number, value: any) {
    set(this.filters, field, value)
    console.log('onFilterCHange', field, value)
    const output = cloneDeep(this.filters)
    this.cleanUndefineds(output)
    this.filterChange.emit(output);
  }

  cleanUndefineds(obj) {
    for (let key in obj) {
      if (obj[key] === undefined) {
        delete obj[key]
      }
      if (typeof obj[key] === 'object') {
        this.cleanUndefineds(obj[key])
        if (!Object.keys(obj[key]).length) {
          delete obj[key]
        }
      }
    }
  }

  onDateChange(field: string | number, event: any) {
    console.log(event)
    this.onFilterChange(field, event.value);
  }

  onDateRangeChange(field: string | number, event: any){//{ start: Date | null; end: Date | null }) {
    console.log(event)
    this.onFilterChange(field, event.value);
  }
}
