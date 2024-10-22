import { ChangeDetectionStrategy, Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import TariffListComponent from '../tariff-list/tariff-list.component';

@Component({
  selector: 'app-change-tariff',
  standalone: true,
  imports: [CommonModule, TariffListComponent],
  templateUrl: './change-tariff.component.html',
  styleUrl: './change-tariff.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class ChangeTariffComponent  {

}
