import { CommonModule } from "@angular/common";
import { ChangeDetectionStrategy, Component, inject, OnInit, TemplateRef, viewChild } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { AbonentsService } from "../../services/abonents.service";
import { forkJoin, map, merge, of, shareReplay, switchMap } from "rxjs";
import { MatTabsModule } from '@angular/material/tabs'
import { NgxDatatableModule, TableColumn } from "@siemens/ngx-datatable";
import { ACCOUNT_TYPES } from "../../../accounts/models/AccountType.enum";
import { PaymentsService } from "../../../payments/services/payments.service";
import { PAYMENT_TYPES } from "../../../payments/models/PaymentType.enum";
import { IInvoice } from "../../../invoices/models/Invoice";
import { NgLetDirective } from "@angular-monorepo/infrastructure";
import { SearchResponse } from "apps/manager-app/src/app/modules/shared/models/SearchResponse.model";
import { User } from "../../models/User.model";

@Component({
  selector: 'app-abonent-page',
  standalone: true,
  templateUrl: './abonent-page.component.html',
  styleUrl: './abonent-page.component.scss',
  imports: [CommonModule, MatTabsModule, NgxDatatableModule, NgLetDirective],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class AbonentPageComponent {
  dateTemplate = viewChild.required<TemplateRef<any>>('dateTemplate');
  accountsLinkTemplate = viewChild.required<TemplateRef<any>>('accountsLinkTemplate');

  activeRoute = inject(ActivatedRoute)
  router = inject(Router)
  abonentsS = inject(AbonentsService)
  paymentsS = inject(PaymentsService)

  abonent$ = this.activeRoute
    .queryParamMap
    .pipe(
      switchMap(queryParamMap =>
        this.abonentsS.search$(0, 1, { userId: queryParamMap.get('id') })
      ),
      map((result) => result.items[0]),
      shareReplay(),
    )

  payments$ = this.abonent$
    .pipe(
      switchMap((abonent) => {
        if (!abonent) return of([{ items: [], itemsCount: 0 }])
        const { accounts } = abonent
        return forkJoin(accounts.map(({ id }) => {
          return this.paymentsS.search$(0, 100, { accountId: id })
        }))
      }),
      map((results: any) => {
        return results.flatMap(res => res.items)
      }),
      shareReplay(),
    )

  accountColumns: TableColumn[] = []

  paymentColumns: TableColumn[] = []

  copyToClipboard(text: string) {
    navigator.clipboard.writeText(text)
  }

  ngAfterViewInit(): void {
    this.accountColumns = [
      {
        prop: 'id',
        name: 'ID',
        sortable: false,
        //minWidth: 400,
        cellTemplate: this.accountsLinkTemplate(),
      },
      {
        prop: 'number',
        name: 'ЛС',
        sortable: false,
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

    this.paymentColumns = [
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
        name: 'Счет',
        pipe: { transform(invoice: IInvoice) { return invoice.id ?? '-' } }
      },
      {
        prop: 'dateTime',
        name: 'Время',
        cellTemplate: this.dateTemplate()
      },
    ]
  }
}
