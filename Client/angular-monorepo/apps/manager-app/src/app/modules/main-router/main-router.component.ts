import { ChangeDetectionStrategy, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav'
import { MatToolbarModule } from '@angular/material/toolbar'
import { MatButtonModule } from '@angular/material/button'

const NAVIGATOR_ROUTES_VISUALS = {
  'services': 'Услуги',
  'managers': 'Менеджеры',
  'invoices': 'Счета',
  'payments': 'Оплаты',
  'abonents': 'Абоненты',
  'accounts': 'Лицевые счета'
}

const ROUTE_NAMES = [
  'abonents',
  'accounts',
  'payments',
  'invoices',
  'services',
  'managers',
]

@Component({
  selector: 'app-main',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatSidenavModule,
    MatToolbarModule,
    MatButtonModule,
  ],
  templateUrl: './main-router.component.html',
  styleUrl: './main-router.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class MainComponent {

  navigatorRoutes = ROUTE_NAMES
  navigatorRoutesVisuals = NAVIGATOR_ROUTES_VISUALS as Record<typeof ROUTE_NAMES[number], string>


}
