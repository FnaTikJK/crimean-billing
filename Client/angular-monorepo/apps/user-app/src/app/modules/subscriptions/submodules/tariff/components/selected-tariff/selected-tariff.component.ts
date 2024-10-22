import { ChangeDetectionStrategy, Component, inject, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ITariffSubscription } from '../../../../models/ITariffSubscription';
import { MatIcon } from '@angular/material/icon';
import { MatRipple } from '@angular/material/core';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-selected-tariff',
  standalone: true,
  imports: [CommonModule, MatIcon, MatRipple, RouterLink],
  templateUrl: './selected-tariff.component.html',
  styleUrl: './selected-tariff.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SelectedTariffComponent {

  tariff = input.required<ITariffSubscription>();
  #router = inject(Router);

  protected f() {
    this.#router.navigate(['subscriptions', 'change-tariff'])
  }
}
