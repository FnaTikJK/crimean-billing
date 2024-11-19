import {
  ChangeDetectionStrategy,
  Component,
  computed,
  DestroyRef,
  ElementRef,
  inject,
  OnInit,
  signal,
  viewChild
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { SubscriptionService } from '../../../../../subscriptions/services/subscription.service';
import { AppSettingsService } from '../../../../../shared/services/app-settings.service';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { filter, fromEvent, Subscription, switchMap, tap } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { IPaymentPayerOwn } from '../../models/IPaymentPayerOwn';
import { ISearchPaymentsRequestDTO } from '../../DTO/request/ISearchPaymentsRequestDTO';
import { PaymentType } from '../../models/PaymentType';
import { PaymentsService } from '../../services/payments.service';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatSlideToggle } from '@angular/material/slide-toggle';
import { MatProgressSpinner } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-payments-list',
  standalone: true,
  imports: [CommonModule, MatButton, MatIcon, MatSlideToggle, ReactiveFormsModule, MatProgressSpinner],
  templateUrl: './payments-list.component.html',
  styleUrl: './payments-list.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PaymentsListComponent implements OnInit {
  #paymentsS = inject(PaymentsService);
  #subscriptionS = inject(SubscriptionService);
  #appSettingsS = inject(AppSettingsService);
  #destroyRef = inject(DestroyRef);

  protected paymentsListElement = viewChild.required<ElementRef<HTMLElement>>('paymentsList');

  protected currentPayments = signal<IPaymentPayerOwn[]>([]);
  protected subscription = computed(() => this.#subscriptionS.subscriptionState().entity);
  protected onlyWithdrawalsControl = new FormControl<boolean>(false, { nonNullable: true});
  protected loading = signal(true);


  protected allCurrentPaymentsCount?: number;

  #scrollPaymentsSubscription?: Subscription;

  ngOnInit() {
    this.getInitialPayments$().subscribe();
    this.listenForPaymentsFilterChange();
    this.listenForPaymentsScroll();
  }

  private listenForPaymentsFilterChange() {
    this.onlyWithdrawalsControl.valueChanges
      .pipe(
        tap(() => {
          this.currentPayments.set([]);
          this.allCurrentPaymentsCount = undefined;
        }),
        switchMap((v) =>  this.requestNewPayments$()),
        takeUntilDestroyed(this.#destroyRef)
      ).subscribe();
  }

  private getInitialPayments$() {
    return this.requestNewPayments$();
  }

  private listenForPaymentsScroll() {
    this.#scrollPaymentsSubscription?.unsubscribe();

    const invoicesList = this.paymentsListElement().nativeElement;
    this.#scrollPaymentsSubscription = fromEvent(invoicesList, 'scroll')
      .pipe(
        filter(() => {
          if (this.loading()) { return false; }
          else if (this.currentPayments().length === this.allCurrentPaymentsCount) {
            this.#scrollPaymentsSubscription?.unsubscribe();
            return false;
          }
          return invoicesList.scrollTop + invoicesList.clientHeight >= invoicesList.scrollHeight;
        }),
        switchMap(() => this.requestNewPayments$(this.currentPayments().length))
      ).subscribe();
  }


  private requestNewPayments$(skip: number = 0) {
    this.loading.set(true);

    const searchOptions: ISearchPaymentsRequestDTO = {
      skip,
      take: 30,
      paymentType: this.onlyWithdrawalsControl.value ? PaymentType.Withdrawal : undefined,
      accountId: this.#appSettingsS.appSettingsState().entity!.accountSelected!
    }

    return this.#paymentsS.getPayments$(searchOptions)
      .pipe(
        tap((newPayments) => {
          this.currentPayments.update(currentPayments => [...currentPayments, ...newPayments]);
          this.allCurrentPaymentsCount = (this.allCurrentPaymentsCount ?? 0) + newPayments.length;
          this.loading.set(false);
        })
      );
  }

  protected readonly PaymentType = PaymentType;
}
