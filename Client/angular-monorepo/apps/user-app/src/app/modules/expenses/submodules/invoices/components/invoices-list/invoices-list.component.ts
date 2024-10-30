import {
  ChangeDetectionStrategy,
  Component,
  computed,
  ElementRef,
  inject, OnInit,
  signal,
  viewChild
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { InvoiceService } from '../../../../services/invoice.service';
import { SubscriptionService } from '../../../../../subscriptions/services/subscription.service';
import { AppSettingsService } from '../../../../../shared/services/app-settings.service';
import { IInvoice } from '../../models/Invoice';
import { filter, fromEvent, Subscription, switchMap, tap } from 'rxjs';
import { MatSlideToggle } from '@angular/material/slide-toggle';
import { MatIcon } from '@angular/material/icon';
import { MatButton } from '@angular/material/button';

@Component({
  selector: 'app-invoices-list',
  standalone: true,
  imports: [CommonModule, MatSlideToggle, MatIcon, MatButton],
  templateUrl: './invoices-list.component.html',
  styleUrl: './invoices-list.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class InvoicesListComponent implements OnInit {

  #invoiceS = inject(InvoiceService);
  #subscriptionS = inject(SubscriptionService);
  #appSettingsS = inject(AppSettingsService);

  protected currentInvoices = signal<IInvoice[]>([]);
  protected subscription = computed(() => this.#subscriptionS.subscriptionState().entity);
  protected loading = signal(true);
  protected invoicesListElement = viewChild.required<ElementRef<HTMLElement>>('invoicesList');

  protected allCurrentInvoicesCount?: number;

  #scrollInvoicesSubscription?: Subscription;

  ngOnInit() {
    this.getInitialInvoices$().subscribe();
    this.listenForTabScroll();
  }

  private getInitialInvoices$() {
    return this.requestNewInvoices$();
  }

  private listenForTabScroll() {
    this.#scrollInvoicesSubscription?.unsubscribe();

    const invoicesList = this.invoicesListElement().nativeElement;
    this.#scrollInvoicesSubscription = fromEvent(invoicesList, 'scroll')
      .pipe(
        tap(() => console.log('scroll')),
        filter(() => {
          if (this.loading()) { return false; }
          else if (this.currentInvoices().length === this.allCurrentInvoicesCount) {
            this.#scrollInvoicesSubscription?.unsubscribe();
            return false;
          }
          return invoicesList.scrollTop + invoicesList.clientHeight >= invoicesList.scrollHeight;
        }),
        switchMap(() => this.requestNewInvoices$(this.currentInvoices().length))
      ).subscribe();
  }


  private requestNewInvoices$(skip: number = 0) {
    this.loading.set(true);

    return this.#invoiceS.getInvoices$(
      {
        skip,
        take: 30,
        accountID: this.#appSettingsS.appSettingsState().entity!.accountSelected!
      })
      .pipe(
        tap((newInvoices) => {
          this.currentInvoices.update(currentInvoices => [...currentInvoices, ...newInvoices]);
          this.allCurrentInvoicesCount = (this.allCurrentInvoicesCount ?? 0) + newInvoices.length;
          this.loading.set(false);
        })
      );
  }

}
