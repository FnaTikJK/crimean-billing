import { CommonModule } from "@angular/common";
import { ChangeDetectionStrategy, Component, inject, OnInit, TemplateRef, viewChild } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { AccountsService } from "../../services/accounts.service";
import { forkJoin, map, merge, Observable, of, shareReplay, switchMap } from "rxjs";
import { MatTabsModule } from '@angular/material/tabs'
import { NgxDatatableModule, TableColumn } from "@siemens/ngx-datatable";
import { ACCOUNT_TYPES } from "../../models/AccountType.enum";
import { PaymentsService } from "../../../payments/services/payments.service";
import { PAYMENT_TYPES } from "../../../payments/models/PaymentType.enum";
import { IInvoice } from "../../../invoices/models/Invoice";
import { NgLetDirective } from "@angular-monorepo/infrastructure";
import { AccountWithUser } from "../../models/DTO/AccountWithUser.model";

@Component({
  selector: 'app-account-page',
  standalone: true,
  templateUrl: './account-page.component.html',
  styleUrl: './account-page.component.scss',
  imports: [CommonModule, MatTabsModule, NgxDatatableModule, NgLetDirective],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class AccountPageComponent {
  dateTemplate = viewChild.required<TemplateRef<any>>('dateTemplate');
  accountsLinkTemplate = viewChild.required<TemplateRef<any>>('accountsLinkTemplate');

  activeRoute = inject(ActivatedRoute)
  router = inject(Router)
  accountsS = inject(AccountsService)
  paymentsS = inject(PaymentsService)

  account$: Observable<AccountWithUser & { accountTypeName?: string }> = this.activeRoute
    .queryParamMap
    .pipe(
      switchMap(queryParamMap =>
        this.accountsS.search$(0, 1, { userId: queryParamMap.get('id') ?? undefined })
      ),
      map((result) => result.items[0]),
      map(result => {
        (result as (typeof result) & { accountTypeName?: string }).accountTypeName = ACCOUNT_TYPES.find(type => type.value === result.accountType)?.name
        return result
      }),
      shareReplay(),
    )

  payments$ = this.account$
    .pipe(
      switchMap((account) => {
        if (!account) return of([{ items: [], itemsCount: 0 }])
        const { id } = account
        return this.paymentsS.search$(0, 100, { accountId: id })
        .pipe(map(results => results.items))
      }),
      shareReplay(),
    )

  accountColumns: TableColumn[] = []

  paymentColumns: TableColumn[] = []
  ACCOUNT_TYPES = ACCOUNT_TYPES;

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
