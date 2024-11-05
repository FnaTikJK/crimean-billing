import { ChangeDetectionStrategy, Component, computed, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IServiceAmount } from '../../../service/models/IServiceAmount';
import { UnitNamePipe } from '../../../../../shared/pipes/unit-name.pipe';
import { MatProgressBar } from '@angular/material/progress-bar';
import { NgLetDirective } from '@angular-monorepo/infrastructure';

@Component({
  selector: 'app-service-left',
  standalone: true,
  imports: [CommonModule, UnitNamePipe, MatProgressBar, NgLetDirective],
  templateUrl: './service-left.component.html',
  styleUrl: './service-left.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ServiceLeftComponent {

  service = input.required<IServiceAmount>();
  paymentDate = input.required<string>();

  protected daysLeft = computed(() => {
    const paymentDate = this.paymentDate();
    const millisecondsPerDay = 24 * 60 * 60 * 1000;
    return Math.ceil((+new Date(paymentDate) - +new Date()) / millisecondsPerDay);
  })
}
