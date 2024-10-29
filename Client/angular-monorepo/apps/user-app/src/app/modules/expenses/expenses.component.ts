import {
  ChangeDetectionStrategy,
  Component, computed,
  effect, ElementRef,
  inject,
  Injector,
  OnInit,
  signal,
  viewChild
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTab, MatTabContent, MatTabGroup } from '@angular/material/tabs';
import { filter, fromEvent, Subscription, switchMap, tap } from 'rxjs';
import { IInvoice } from './models/Invoice';
import { InvoiceService } from './services/invoice.service';
import { AppSettingsService } from '../shared/services/app-settings.service';
import { MatCard } from '@angular/material/card';
import { MatIcon } from '@angular/material/icon';
import { SubscriptionService } from '../subscriptions/services/subscription.service';
import { MatSlideToggle } from '@angular/material/slide-toggle';

@Component({
  selector: 'app-expenses',
  standalone: true,
  imports: [CommonModule, MatTabGroup, MatTab, MatTabContent, MatCard, MatIcon, MatSlideToggle],
  templateUrl: './expenses.component.html',
  styleUrl: './expenses.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class ExpensesComponent implements OnInit {

  #invoiceS = inject(InvoiceService);
  #subscriptionS = inject(SubscriptionService);
  #appSettingsS = inject(AppSettingsService);
  #injector = inject(Injector);

  protected currentTabItems = signal<IInvoice[]>([]);
  protected subscription = computed(() => this.#subscriptionS.subscriptionState().entity);
  protected allCurrentTabItemsCount?: number;

  protected currentTabIndex = signal<ExpensesTabs>(ExpensesTabs.all);
  protected loading = signal(true);

  protected allWrapperElement = viewChild.required<ElementRef<HTMLDivElement>>('allWrapper');
  protected replenishWrapperElement = viewChild<ElementRef<HTMLDivElement>>('replenishWrapper');
  protected expensesWrapperElement = viewChild<ElementRef<HTMLDivElement>>('expensesWrapper');

  #scrollTabSubscription?: Subscription;

  ngOnInit() {
    this.listenForTabChange();
  }

  private getInitialTabItems$() {
    this.currentTabItems.set([]);
    this.allCurrentTabItemsCount = undefined;
    return this.requestNewItems$();
  }
  private listenForTabChange() {
    effect(() => {
      this.currentTabIndex();
      this.getInitialTabItems$()
        .subscribe(() => this.listenForTabScroll());
    }, { injector: this.#injector, allowSignalWrites: true });
  }
  private listenForTabScroll() {
    this.#scrollTabSubscription?.unsubscribe();

    const tabElement = this.getCurrentTabElement();
    if (!tabElement) { return; }

    this.#scrollTabSubscription = fromEvent(tabElement!, 'scroll')
      .pipe(
        filter(() => {
          if (this.loading()) { return false; }
          else if (this.currentTabItems().length === this.allCurrentTabItemsCount) {
            this.#scrollTabSubscription?.unsubscribe();
            return false;
          }
          return tabElement!.scrollTop + tabElement!.clientHeight >= tabElement!.scrollHeight;
        }),
        switchMap(() => this.requestNewItems$(this.currentTabItems().length))
      ).subscribe();
  }

  private getCurrentTabElement() {
    const currentTabIndex = this.currentTabIndex();
    switch (currentTabIndex) {
      case ExpensesTabs.all:
        return this.allWrapperElement().nativeElement;
      case ExpensesTabs.expenses:
        return this.expensesWrapperElement()?.nativeElement;
      case ExpensesTabs.replenish:
        return this.replenishWrapperElement()?.nativeElement;
      }
  }

  private requestNewItems$(skip: number = 0) {
    this.loading.set(true);

    return this.#invoiceS.getInvoices$(
      {
        skip,
        take: 30,
        accountID: this.#appSettingsS.appSettingsState().entity!.accountSelected!
      })
      .pipe(
        tap((newItems) => {
          this.currentTabItems.update(currentItems => [...currentItems, ...newItems]);
          this.allCurrentTabItemsCount = (this.allCurrentTabItemsCount ?? 0) + newItems.length;
          this.loading.set(false);
        })
      );
  }

}

enum ExpensesTabs {
  all ,
  replenish,
  expenses
}
