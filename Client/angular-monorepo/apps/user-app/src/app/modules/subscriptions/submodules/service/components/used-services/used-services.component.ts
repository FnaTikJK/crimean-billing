import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SubscriptionService } from '../../../../services/subscription.service';
import { MatIcon } from '@angular/material/icon';
import { UnitNamePipe } from '../../../../../shared/pipes/unit-name.pipe';
import { MatButton } from '@angular/material/button';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-used-services',
  standalone: true,
  imports: [CommonModule, MatIcon, UnitNamePipe, MatButton, RouterLink],
  templateUrl: './used-services.component.html',
  styleUrl: './used-services.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class UsedServicesComponent {

  private subscriptionS = inject(SubscriptionService);

  protected usedServices = this.subscriptionS.subscriptionState().entity?.serviceUsages;
}
