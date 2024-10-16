import { ChangeDetectionStrategy, Component, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountCardComponent } from './components/account-card/account-card.component';
import { AppSettingsService } from '../shared/services/app-settings.service';

@Component({
  selector: 'app-main',
  standalone: true,
  imports: [CommonModule, AccountCardComponent],
  templateUrl: './main.component.html',
  styleUrl: './main.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class MainComponent {

  #appSettingsS = inject(AppSettingsService);

  protected accountIDToDisplay = computed(() => this.#appSettingsS.appSettingsState().entity?.accountSelected);
}
