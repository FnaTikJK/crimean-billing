import { ChangeDetectionStrategy, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import TariffListComponent from '../tariff-list/tariff-list.component';

@Component({
  selector: 'app-add-tariff',
  standalone: true,
  imports: [CommonModule, TariffListComponent],
  templateUrl: './add-tariff.component.html',
  styleUrl: './add-tariff.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class AddTariffComponent {}
