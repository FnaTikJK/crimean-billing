import {
  ChangeDetectionStrategy,
  Component,
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
import { PaymentType } from '../../../../../expenses/submodules/payments/models/PaymentType';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { MatListOption, MatSelectionList } from '@angular/material/list';
import {
  MatExpansionModule,
} from '@angular/material/expansion';
import { MatIcon } from '@angular/material/icon';
import { UnitNamePipe } from '../../../../../shared/pipes/unit-name.pipe';
import { ServiceType } from '../../models/ServiceType';
import { MatButton } from '@angular/material/button';
import { FormControl, ReactiveFormsModule } from '@angular/forms';


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

  protected servicesListElement = viewChild.required<ElementRef<HTMLElement>>('servicesList');

  protected newSelectedServicesControl = new FormControl<string[]>([], {nonNullable: true});
  protected currentServices = signal<IService[]>([]);
  protected loading = signal(true);


  protected serviceTypeToIconName: Record<ServiceType, string> = {
    [ServiceType.Internet]: 'wifi',
    [ServiceType.SMS]: 'sms',
    [ServiceType.MMS]: 'mms',
    [ServiceType.Calls]: 'call'
  }


  protected allServicesCount?: number;

  #scrollServicesSubscription?: Subscription;

  ngOnInit() {
    this.getInitialServices$().subscribe();
    this.listenForServicesScroll();
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


  private requestNewServices$(skip: number = 0) {
    this.loading.set(true);

    const searchOptions: ISearchServiceRequestDTO = {
      skip,
      take: 30
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

  protected readonly PaymentType = PaymentType;
}
