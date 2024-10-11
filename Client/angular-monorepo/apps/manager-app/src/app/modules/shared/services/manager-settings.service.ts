import { inject, Injectable, signal } from '@angular/core';
import { LocalStorageService } from './local-storage.service';

@Injectable({
  providedIn: 'root'
})
export class ManagerSettingsService {

  #localStorageS = inject(LocalStorageService);

  constructor() {
    this.applySettings();
    this.listenForTabClose();
  }

  #savedManagerSettings = signal<IManagerSettings>({});
  savedManagerSettings = this.#savedManagerSettings.asReadonly();

  updateSettings(managerSettings: Partial<IManagerSettings>) {
    this.#savedManagerSettings.update(settings => ({...settings, ...managerSettings}));
  }
  private applySettings(): void {
    const settings: IManagerSettings | null = this.#localStorageS.get('managerSettings');
    if (settings) { this.#savedManagerSettings.set(settings); }
  }

  private listenForTabClose() {
    window.addEventListener('beforeunload', () => {
      if (Object.keys(this.#savedManagerSettings()).length) {
        this.#localStorageS.set('managerSettings', this.#savedManagerSettings());
      }
    });
  }
}

interface IManagerSettings {
  saveCredentials?: boolean;
}
