import { ChangeDetectionStrategy, Component, computed, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import TariffListComponent from '../tariff-list/tariff-list.component';
import { SubscriptionService } from '../../../../services/subscription.service';

@Component({
  selector: 'app-change-tariff',
  standalone: true,
  imports: [CommonModule, TariffListComponent],
  templateUrl: './change-tariff.component.html',
  styleUrl: './change-tariff.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class ChangeTariffComponent  {

  #subscriptionS = inject(SubscriptionService);

  protected tariffToChangeID = computed(() => {
    const subscription = this.#subscriptionS.subscriptionState().entity;
    return subscription?.preferredTariff?.templateId ?? subscription?.tariff?.templateId
  });
}
