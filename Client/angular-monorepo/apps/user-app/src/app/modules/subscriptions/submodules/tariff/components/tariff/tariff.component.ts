import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButton } from '@angular/material/button';
import { SubscriptionService } from '../../../../services/subscription.service';
import { AppSettingsService } from '../../../../../shared/services/app-settings.service';

@Component({
  selector: 'app-tariff',
  standalone: true,
  imports: [CommonModule, MatButton],
  templateUrl: './tariff.component.html',
  styleUrl: './tariff.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class TariffComponent {

  #subscriptionS = inject(SubscriptionService);
  #appSettingsS = inject(AppSettingsService);
  protected changeTariff() {
    // this.#subscriptionS.changeSubscription$({ tariffTemplateId:  ,accountId: this.#appSettingsS.appSettingsState().entity })
  }
}
