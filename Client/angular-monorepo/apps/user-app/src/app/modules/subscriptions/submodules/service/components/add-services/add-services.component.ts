import {
  ChangeDetectionStrategy,
  Component, DestroyRef,
  ElementRef,
  inject, OnInit,
  signal,
  viewChild
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { filter, fromEvent, map, Subscription, switchMap, tap } from 'rxjs';
import { IService } from '../../models/IService';
import { ISearchServiceRequestDTO } from '../../DTO/request/ISearchServiceRequestDTO';
import { ServiceService } from '../../services/service.service';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { MatListOption, MatSelectionList } from '@angular/material/list';
import {
  MatExpansionModule,
} from '@angular/material/expansion';
import { MatIcon } from '@angular/material/icon';
import { UnitNamePipe } from '../../../../../shared/pipes/unit-name.pipe';
import { MatButton } from '@angular/material/button';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { SubscriptionService } from '../../../../services/subscription.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatBottomSheet } from '@angular/material/bottom-sheet';
import { ModalConfirmComponent } from '../../../../../shared/modals/modal-confirm.component';
import { MatSnackBar } from '@angular/material/snack-bar';
import { serviceTypeToIconName } from '../../../../../shared/entities/ServiceTypeToIcon';


@Component({
  selector: 'app-add-services',
  standalone: true,
  imports: [CommonModule, MatProgressSpinner, MatListOption, MatSelectionList, MatExpansionModule, MatIcon, UnitNamePipe, MatButton, ReactiveFormsModule],
  templateUrl: './add-services.component.html',
  styleUrl: './add-services.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class AddServicesComponent implements OnInit {

  #servicesS = inject(ServiceService);
  #subscriptionS = inject(SubscriptionService);

  #matBottomSheet = inject(MatBottomSheet);
  #snackbar = inject(MatSnackBar);
  #destroyRef = inject(DestroyRef);

  protected servicesListElement = viewChild.required<ElementRef<HTMLElement>>('servicesList');

  protected newSelectedServicesControl = new FormControl<string[]>([], {nonNullable: true});
  protected currentAddCost = signal(0);
  protected currentServices = signal<IService[]>([]);
  protected loading = signal(true);

  protected allServicesCount?: number;

  #scrollServicesSubscription?: Subscription;

  ngOnInit() {
    this.getInitialServices$().subscribe();
    this.listenForServicesScroll();
    this.listenForAddServicesCheckChange();
  }

  private getInitialServices$() {
    return this.requestNewServices$();
  }

  private listenForServicesScroll() {
    this.#scrollServicesSubscription?.unsubscribe();

    const invoicesList = this.servicesListElement().nativeElement;
    this.#scrollServicesSubscription = fromEvent(invoicesList, 'scroll')
      .pipe(
        filter(() => {
          if (this.loading()) { return false; }
          else if (this.currentServices().length === this.allServicesCount) {
            this.#scrollServicesSubscription?.unsubscribe();
            return false;
          }
          return invoicesList.scrollTop + invoicesList.clientHeight >= invoicesList.scrollHeight;
        }),
        switchMap(() => this.requestNewServices$(this.currentServices().length))
      ).subscribe();
  }

  private listenForAddServicesCheckChange() {
    this.newSelectedServicesControl.valueChanges
      .pipe(
        tap(value => {
          const checkedServices = value.map(serviceID => this.currentServices().find(service => service.id === serviceID) as IService);
          const checkedServicesPrices = checkedServices.map(service => service.price);
          return this.currentAddCost.set(checkedServicesPrices.reduce((acc, price) => acc += price, 0));
        }),
        takeUntilDestroyed(this.#destroyRef)
      ).subscribe();
  }

  private reload$() {
    this.newSelectedServicesControl.setValue([]);
    this.currentServices.set([]);
    return this.requestNewServices$();
  }


  private requestNewServices$(skip: number = 0) {
    this.loading.set(true);
    const servicesToExclude = this.#subscriptionS.subscriptionState().entity!.serviceUsages
      .map(service => service.serviceTemplateId);


    const searchOptions: ISearchServiceRequestDTO = {
      skip,
      take: 30,
      excludedTemplateIds: servicesToExclude,
      isTariffService: false
    }

    return this.#servicesS.getServices$(searchOptions)
      .pipe(
        tap(searchResponse => {
          if (!this.allServicesCount) { this.allServicesCount = searchResponse.totalCount; }
        }),
        map(res => res.items as IService[]),
        tap((newServices) => {
          this.currentServices.update(currentServices => [...currentServices, ...newServices]);
          this.loading.set(false);
        })
      );
  }

  protected openConfirmModal() {
    this.#matBottomSheet.open(ModalConfirmComponent,
      { data: `Вы уверены, что хотите подключить выбранные услуги? Ежемесячная плата увеличится на ${this.currentAddCost()}₽`
      })
      .afterDismissed()
      .pipe(
        filter(res => !!res),
        switchMap(() => this.#subscriptionS.addServicesToSubscription$(this.newSelectedServicesControl.value)),
        tap(() => this.#snackbar.open("Сервисы успешно добавлены", 'Закрыть', { duration: 1500, verticalPosition: 'top' })),
        switchMap(() => this.reload$())
      ).subscribe()
  }

  protected readonly serviceTypeToIconName = serviceTypeToIconName;
}
