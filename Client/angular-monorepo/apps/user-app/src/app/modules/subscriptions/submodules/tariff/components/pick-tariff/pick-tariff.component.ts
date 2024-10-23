import { ChangeDetectionStrategy, Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButton } from '@angular/material/button';
import { SubscriptionService } from '../../../../services/subscription.service';
import { AppSettingsService } from '../../../../../shared/services/app-settings.service';
import { ITariff } from '../../models/ITariff';
import { ActivatedRoute, Router } from '@angular/router';
import { filter, map, switchMap, tap } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { TariffService } from '../../services/tariff.service';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import {
  MatCell,
  MatCellDef,
  MatColumnDef,
  MatHeaderCell,
  MatHeaderCellDef,
  MatHeaderRow, MatHeaderRowDef, MatRow, MatRowDef,
  MatTable
} from '@angular/material/table';
import { ServiceNamePipe } from '../../../../../shared/pipes/service-name.pipe';
import { UnitNamePipe } from '../../../../../shared/pipes/unit-name.pipe';

@Component({
  selector: 'app-pick-tariff',
  standalone: true,
  imports: [CommonModule, MatButton, MatProgressSpinner, MatTable, MatHeaderCellDef, MatCell, MatCellDef, MatHeaderCell, MatColumnDef, ServiceNamePipe, UnitNamePipe, MatHeaderRow, MatRow, MatRowDef, MatHeaderRowDef],
  templateUrl: './pick-tariff.component.html',
  styleUrl: './pick-tariff.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class PickTariffComponent implements OnInit {

  #subscriptionS = inject(SubscriptionService);
  #tariffS = inject(TariffService);
  #appSettingsS = inject(AppSettingsService);
  #activatedRoute = inject(ActivatedRoute);
  #router = inject(Router);
  #destroyRef = inject(DestroyRef);

  tariff = signal<ITariff | null>(null);
  columnsToDisplay = ['Type', 'Units']

  loading = signal(true);

  ngOnInit() {
    this.extractTariffTemplateIDFromURL();
  }

  protected subscribeToTariff() {
    this.#subscriptionS.changeSubscription$({
      tariffTemplateId: this.tariff()!.templateId, accountId: this.#appSettingsS.appSettingsState().entity!.accountSelected!
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
      ).subscribe(tariff => this.tariff.set(tariff));
  }
}
