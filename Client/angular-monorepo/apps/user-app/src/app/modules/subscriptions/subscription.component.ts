import { ChangeDetectionStrategy, Component, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SubscriptionService } from './services/subscription.service';
import { SelectedTariffComponent } from './submodules/tariff/components/selected-tariff/selected-tariff.component';
import { MatButton } from '@angular/material/button';
import { RouterLink } from '@angular/router';


@Component({
  selector: 'app-subscription',
  standalone: true,
  imports: [CommonModule, SelectedTariffComponent, MatButton, RouterLink],
  templateUrl: './subscription.component.html',
  styleUrl: './subscription.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class SubscriptionComponent {

  #subscriptionS = inject(SubscriptionService);

  protected subscription = computed(() => this.#subscriptionS.subscriptionState().entity);
}
