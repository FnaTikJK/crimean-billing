import { AfterViewChecked, AfterViewInit, ChangeDetectionStrategy, ChangeDetectorRef, Component, DestroyRef, inject, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { BehaviorSubject, catchError, combineLatest, debounceTime, filter, map, Observable, of, switchMap, tap } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop'
import { DatatableComponent, NgxDatatableModule, SortDirection, TableColumn } from '@siemens/ngx-datatable'
import ListFilterComponent, { FilterField } from './components/list-filter/list-filter.component';
import { debounce, isNil } from 'lodash';

@Component({
  selector: 'app-base-list-with-filters',
  standalone: true,
  template: '',
  styles: [],
  imports: [CommonModule, RouterModule, ListFilterComponent, NgxDatatableModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default abstract class BaseListWithFiltersComponent implements AfterViewInit {
  @ViewChild('table') table!: DatatableComponent
  @ViewChild('appListFilter') appListFilter!: ListFilterComponent
  cdr = inject(ChangeDetectorRef)
  private destroyRef = inject(DestroyRef)
  abstract service: { search$(skip: number, take: number, filter: any): Observable<any> }
  PAGE_SIZE = 100

  filterFields: FilterField[] = []
  columns: TableColumn[] = []
  fieldToOrder = {}

  curOrderBy$ = new BehaviorSubject(null)
  curOrderDir$ = new BehaviorSubject(null)
  curOrder$ = combineLatest([
    this.curOrderBy$,
    this.curOrderDir$,
  ]).pipe(
    map(([orderBy, orderDir]) => {
      const order = {} as any
      if (!isNil(orderBy)) {
        order.orderBy = orderBy
      }
      if (!isNil(orderDir)) {
        order.orderDir = orderDir
      }
      return order
    })
  )
  pagination$ = new BehaviorSubject({ skip: 0, take: this.PAGE_SIZE })
  initialFilter = {}
  curFilter$ = new BehaviorSubject(this.initialFilter)

  isLoading = false
  totalCount = 0
  loadedCount = 0
  initialLoad = true
  items: any[] = []


  load$ = combineLatest([
    this.curFilter$,
    this.pagination$,
    this.curOrder$,
  ]).pipe(
    debounceTime(500),
    filter(() => {
      if (this.initialLoad) {
        return true
      }
      return this.totalCount > this.loadedCount
    }),
    tap(() => this.isLoading = true),
    switchMap(([curFilter, { skip, take }, orderFields = {}]) =>
      this.service.search$(skip, take, {
        ...curFilter,
        ...orderFields
      })
        .pipe(
          tap((resp) => this.totalCount = resp.totalCount),
          tap(resp => {
            this.items = [...this.items, ...resp.items]
            this.cdr.detectChanges()
          }),
        )
    ),
    tap(() => this.isLoading = false),
    catchError(() => {
      this.isLoading = false
      return of([])
    })
  )

  ngAfterViewInit(): void {
    this.load$
      .pipe(
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe(() => console.log('load$'))

    if (this.table && this.appListFilter) {
      this.table.element.style.height = `calc(100vh - ${this.appListFilter.container.nativeElement.offsetHeight}px - 48px)`
    }
  }


  onScroll(ev) {
    if (ev.target) {
      const leftToScroll = +ev.target.scrollHeight - +ev.target.clientHeight - +ev.target.scrollTop
      console.error(leftToScroll)
      if (leftToScroll < 100) {
        let { skip, take } = this.pagination$.value
        skip = take
        take += this.PAGE_SIZE
        this.pagination$.next({ skip, take })
      }
    }
  }

  changeFilter(event: any) {
    console.log(event)
    this.initialLoad = true
    this.items = []
    this.pagination$.next({ skip: 0, take: this.PAGE_SIZE })
    this.curFilter$.next(event)
  }

  onColumnSort(event: any) {
    const prop = event.column.prop
    let sort = event.newValue
    if (!isNil(sort)) {
      sort = SORT_DIR_TO_DTO[sort]
    }
    this.items = []
    this.pagination$.next({ skip: 0, take: this.PAGE_SIZE })
    this.curOrderBy$.next(prop)
    this.curOrderDir$.next(sort)
  }
}

const SORT_DIR_TO_DTO = {
  'asc': SortDirection.asc,
  'desc': SortDirection.desc,
}
