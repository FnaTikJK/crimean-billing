import { inject, Injectable } from '@angular/core';
import { HttpService } from '@angular-monorepo/infrastructure';
@Injectable({
  providedIn: 'root'
})
export class AuthorizationService {

  #httpS = inject(HttpService);

  login$(phoneNumber: string) {
    return this.#httpS.post('Auth/Login', { phoneNumber });
  }
}
