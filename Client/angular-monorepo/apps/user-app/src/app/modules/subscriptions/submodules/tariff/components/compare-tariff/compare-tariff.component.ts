import { ChangeDetectionStrategy, Component, computed, DestroyRef, inject, Signal, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButton } from '@angular/material/button';
import {
  MatCell,
  MatCellDef,
  MatColumnDef,
  MatHeaderCell, MatHeaderCellDef,
  MatHeaderRow,
  MatHeaderRowDef, MatRow, MatRowDef, MatTable
} from '@angular/material/table';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { ServiceNamePipe } from '../../../../../shared/pipes/service-name.pipe';
import { UnitNamePipe } from '../../../../../shared/pipes/unit-name.pipe';
import { SubscriptionService } from '../../../../services/subscription.service';
import { TariffService } from '../../services/tariff.service';
import { AppSettingsService } from '../../../../../shared/services/app-settings.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ITariff } from '../../models/ITariff';
import { filter, map, switchMap, tap } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ServiceType } from '../../../service/models/ServiceType';
import { UnitType } from '../../../service/models/UnitType';

@Component({
  selector: 'app-compare-tariff',
  standalone: true,
  imports: [CommonModule, MatButton, MatCell, MatCellDef, MatColumnDef, MatHeaderCell, MatHeaderRow, MatHeaderRowDef, MatProgressSpinner, MatRow, MatRowDef, MatTable, ServiceNamePipe, UnitNamePipe, MatHeaderCellDef],
  templateUrl: './compare-tariff.component.html',
  styleUrl: './compare-tariff.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class CompareTariffComponent {


  #subscriptionS = inject(SubscriptionService);
  #tariffS = inject(TariffService);
  #appSettingsS = inject(AppSettingsService);
  #activatedRoute = inject(ActivatedRoute);
  #router = inject(Router);
  #destroyRef = inject(DestroyRef);

  protected currentTariff = computed(() => this.#subscriptionS.subscriptionState().entity?.tariff);
  protected preferredTariff = signal<ITariff | null>(null);
  protected columnsToDisplay = ['Type' , 'CurrentTariff', 'PreferredTariff'];
  protected rows = this.getRows();

  protected loading = signal(true);

  ngOnInit() {
    this.extractTariffTemplateIDFromURL();
  }

  protected changeTariff() {
    this.#subscriptionS.changeSubscription$({
      tariffTemplateId: this.preferredTariff()!.templateId,
      accountId: this.#appSettingsS.appSettingsState().entity!.accountSelected!
    })
      .pipe(
        tap(() => this.#router.navigate(['subscriptions'], { relativeTo: null }))
      )
      .subscribe()
  }

  private extractTariffTemplateIDFromURL() {
    this.#activatedRoute.paramMap
      .pipe(
        map(params => params.get('id')),
        filter(id => !!id),
        tap(() => this.loading.set(true)),
        switchMap((tariffID) => this.#tariffS.getTariff$(tariffID as string)),
        tap(() => this.loading.set(false)),
        takeUntilDestroyed(this.#destroyRef)
      ).subscribe(tariff => this.preferredTariff.set(tariff));
  }

  private getRows(): Signal<ITariffRow[] | null> {
    return computed(() => {
      if (!this.currentTariff() || !this.preferredTariff()) { return []; }

      const currentTariffServices = this.currentTariff()!.services ?? [];
      const preferredTariffServices = this.preferredTariff()!.services ?? [];

      const uniqueServices = new Set([
        ...currentTariffServices.map(s => s.serviceType),
        ...preferredTariffServices.map(s => s.serviceType)
      ]);

      return [...uniqueServices]
        .map(serviceType => {
          const currentTariffAmount = currentTariffServices.find(s => s.serviceType === serviceType);
          const preferredTariffAmount = preferredTariffServices.find(s => s.serviceType === serviceType);

          return {
            serviceType,
            currentTariff: currentTariffAmount ? { amount: currentTariffAmount.amount, unitType: currentTariffAmount.unitType } : undefined,
            preferredTariff: preferredTariffAmount ? { amount: preferredTariffAmount.amount, unitType: preferredTariffAmount.unitType } : undefined,
          } as ITariffRow;
        })
    })
  }
}

interface ITariffRow {
  serviceType: ServiceType;
  currentTariff: IServiceAmount;
  preferredTariff: IServiceAmount;
}

interface IServiceAmount {
  amount: number;
  unitType: UnitType;
}
