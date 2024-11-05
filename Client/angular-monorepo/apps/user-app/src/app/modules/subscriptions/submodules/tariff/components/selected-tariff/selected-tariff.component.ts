import { ChangeDetectionStrategy, Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ITariffSubscription } from '../../../../models/ITariffSubscription';
import { MatIcon } from '@angular/material/icon';
import { MatRipple } from '@angular/material/core';
import {RouterLink } from '@angular/router';
import { ISubscription } from '../../../../models/ISubscription';
import { ServiceLeftComponent } from '../service-left/service-left.component';

@Component({
  selector: 'app-selected-tariff',
  standalone: true,
  imports: [CommonModule, MatIcon, MatRipple, RouterLink, ServiceLeftComponent],
  templateUrl: './selected-tariff.component.html',
  styleUrl: './selected-tariff.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SelectedTariffComponent {

  subscription = input.required<ISubscription>();
}
