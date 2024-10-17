import { effect, inject, Injectable, signal } from '@angular/core';
import { HttpService } from '@angular-monorepo/infrastructure';
import { ReplaySubject, switchMap, tap } from 'rxjs';
import { ProfileService } from '../../profile/services/profile.service';
@Injectable({
  providedIn: 'root'
})
export class AuthorizationService {

  #httpS = inject(HttpService);
  #profileS = inject(ProfileService);

  #isAuthorized =  signal(false);
  isAuthorized = this.#isAuthorized.asReadonly();

  #authorizationChecked$ = new ReplaySubject<boolean>(1);
  authorizationChecked$ = this.#authorizationChecked$.asObservable();

  constructor() {
    this.checkSavedAuthorization();
  }

  login$(phoneNumber: string) {
    return this.#httpS.post('Auth/Login', { phoneNumber });
  }

  logOut$() {
    return this.#httpS.post('Auth/Logout')
      .pipe(
        tap(() => location.reload())
      );
  }

  sendVerificationCode(verificationCode: string) {
    return this.#httpS.post('Auth/Verify', { verificationCode })
      .pipe(
        switchMap(() => this.#profileS.setupProfile$()),
        tap(() => this.#isAuthorized.set(true))
      );
  }

  private checkSavedAuthorization() {
    const profileInitEffect = effect(() => {
      const profileState = this.#profileS.profileState();
      if (profileState.checked) {
        this.#isAuthorized.set(this.#profileS.profileState().loaded);
        this.#authorizationChecked$.next(true);
        profileInitEffect.destroy();
      }
    }, { allowSignalWrites: true });
  }
}
