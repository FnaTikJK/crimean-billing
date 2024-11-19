import {
  ChangeDetectionStrategy,
  Component,
  computed, DestroyRef,
  ElementRef,
  inject, OnInit,
  signal,
  viewChild
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { InvoiceService } from '../../services/invoice.service';
import { SubscriptionService } from '../../../../../subscriptions/services/subscription.service';
import { AppSettingsService } from '../../../../../shared/services/app-settings.service';
import { IInvoice } from '../../models/Invoice';
import { filter, fromEvent, map, Subscription, switchMap, tap } from 'rxjs';
import { MatSlideToggle } from '@angular/material/slide-toggle';
import { MatIcon } from '@angular/material/icon';
import { MatButton } from '@angular/material/button';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ISearchInvoicesRequestDTO } from '../../DTO/request/SearchInvoicesRequestDTO';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { AccountService } from '../../../../../profile/submodules/account/services/account.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatBottomSheet } from '@angular/material/bottom-sheet';
import {
  AddMoneyBottomSheetComponent
} from '../../../../../main/components/add-money-bottom-sheet/add-money-bottom-sheet.component';
import { IPaymentPayerOwn } from '../../../payments/models/IPaymentPayerOwn';

@Component({
  selector: 'app-invoices-list',
  standalone: true,
  imports: [CommonModule, MatSlideToggle, MatIcon, MatButton, ReactiveFormsModule, MatProgressSpinner],
  templateUrl: './invoices-list.component.html',
  styleUrl: './invoices-list.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class InvoicesListComponent implements OnInit {

  #invoiceS = inject(InvoiceService);
  #subscriptionS = inject(SubscriptionService);
  #accountS = inject(AccountService);
  #appSettingsS = inject(AppSettingsService);
  #matSnackbar = inject(MatSnackBar);
  #matBottomSheet = inject(MatBottomSheet);
  #destroyRef = inject(DestroyRef);

  protected invoicesListElement = viewChild.required<ElementRef<HTMLElement>>('invoicesList');

  protected currentInvoices = signal<IInvoice[]>([]);
  protected subscription = computed(() => this.#subscriptionS.subscriptionState().entity);
  protected onlyNotPaidInvoicesControl = new FormControl<boolean>(false, { nonNullable: true});
  protected loading = signal(true);


  protected allInvoicesCount?: number;

  #scrollInvoicesSubscription?: Subscription;

  ngOnInit() {
    this.getInitialInvoices$().subscribe();
    this.listenForInvoicesFilterChange();
    this.listenForInvoicesScroll();
  }

  protected payForInvoice(invoiceToPay: IInvoice) {

    const sendPayRequest = () => {
      this.#invoiceS.payForInvoice$({invoiceId: invoiceToPay.id}, selectedAccountID as string)
        .pipe(
          tap((payedInvoice) => {
            this.#matSnackbar.open('Инвойс успешно оплачен', 'Закрыть', { duration: 1500 });
            this.changeInvoiceStatus(payedInvoice);
          })
        ).subscribe();
    }

    const selectedAccountID = this.#appSettingsS.appSettingsState().entity?.accountSelected;
    if (!selectedAccountID) { return; }
    const accountEntity = this.#accountS.getID(selectedAccountID);
    if (!accountEntity) { return; }
    else if (accountEntity.money < invoiceToPay.toPay) {
      this.#matSnackbar.open('На счету недостаточно денег. Перед оплатой необходимо пополнить счёт', undefined,
        { duration: 2500, verticalPosition: 'top' });

      const sheetRef = this.#matBottomSheet.open(AddMoneyBottomSheetComponent, {
        data: {
          accountID: selectedAccountID,
          defaultMoneyValue: invoiceToPay.toPay - accountEntity.money,
          showSnackbar: false
        }
      });

      sheetRef.afterDismissed()
        .pipe(
          tap(addedMoney => addedMoney && sendPayRequest())
        ).subscribe();
    } else {
      sendPayRequest();
    }
  }

  private listenForInvoicesFilterChange() {
    this.onlyNotPaidInvoicesControl.valueChanges
      .pipe(
        tap(() => {
          this.currentInvoices.set([]);
          this.allInvoicesCount = undefined;
        }),
        switchMap((v) =>  this.requestNewInvoices$()),
        takeUntilDestroyed(this.#destroyRef)
      ).subscribe();
  }

  private getInitialInvoices$() {
    return this.requestNewInvoices$();
  }

  private listenForInvoicesScroll() {
    this.#scrollInvoicesSubscription?.unsubscribe();

    const invoicesList = this.invoicesListElement().nativeElement;
    this.#scrollInvoicesSubscription = fromEvent(invoicesList, 'scroll')
      .pipe(
        filter(() => {
          if (this.loading()) { return false; }
          else if (this.currentInvoices().length === this.allInvoicesCount) {
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

    const searchOptions: ISearchInvoicesRequestDTO = {
      skip,
      take: 30,
      isPayed: this.onlyNotPaidInvoicesControl.value ? !this.onlyNotPaidInvoicesControl.value : undefined,
      accountID: this.#appSettingsS.appSettingsState().entity!.accountSelected!
    }

    return this.#invoiceS.getInvoices$(searchOptions)
      .pipe(
        tap(searchResponse => {
          if (!this.allInvoicesCount) { this.allInvoicesCount = searchResponse.totalCount; }
        }),
        map(res => res.items as IInvoice[]),
        tap((newInvoices) => {
          this.currentInvoices.update(currentInvoices => [...currentInvoices, ...newInvoices]);
          this.loading.set(false);
        })
      );
  }

  private changeInvoiceStatus(newState: IInvoice) {
    if (this.onlyNotPaidInvoicesControl.value) {
      this.currentInvoices.update(invoices => invoices.filter(i => i.id !== newState.id));
    } else {
      this.currentInvoices.update(invoices => {
        const invoicesCopy = [...invoices];
        let invoiceToModify = invoicesCopy.find(i => i.id === newState.id);
        Object.assign(invoiceToModify!, newState);
        return invoicesCopy;
      });
    }
  }

}
