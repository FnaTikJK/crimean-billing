import { inject, Injectable, signal } from '@angular/core';
import { HttpService } from '../../shared/services/http.service';
import { ILoginManagerRequestDTO } from '../DTO/requests/ILoginManagerRequestDTO';
import { IChangeManagerPasswordRequest } from '../DTO/requests/IChangeManagerPasswordRequest';
import { catchError, map, of, ReplaySubject, tap, throwError } from 'rxjs';
import { ILoginManagerResponseDTO } from '../DTO/response/ILoginManagerResponseDTO';
import { ManagerSettingsService } from '../../shared/services/manager-settings.service';
import { IProfile, ProfileService } from '../../shared/services/profile.service';
import { ProfileRoleType } from '@angular-monorepo/shared-separate-entities';

@Injectable({
  providedIn: 'root'
})
export class AuthorizationService {

  #managerSettingsS = inject(ManagerSettingsService);
  #profileS = inject(ProfileService);
  #httpS = inject(HttpService);

  #isAuthorized = signal(false);
  isAuthorized = this.#isAuthorized.asReadonly();

  #authorizationChecked$ = new ReplaySubject<boolean>(1);
  authorizationChecked$ = this.#authorizationChecked$.asObservable();

  constructor() {
    this.checkSavedAuthorization();
  }

  login$(loginManagerRequest: ILoginManagerRequestDTO) {
    return this.#httpS.post<ILoginManagerResponseDTO>('Auth/Managers/Login', loginManagerRequest)
      .pipe(
        map(userIDObj => ({id: userIDObj.userId, role: ProfileRoleType.Manager})),
        tap((profile) => this.authorize(profile))
      );
  }

  changePassword$(changeManagerPasswordRequest: IChangeManagerPasswordRequest) {
    return this.#httpS.post('Auth/Managers/ChangePassword', changeManagerPasswordRequest)
      .pipe(
        tap({
          next: () => this.unAuthorize(),
          error: () => this.unAuthorize()
        })
      );
  }

  logOut$() {
    return this.#httpS.post('Auth/Logout')
      .pipe(
        tap(() => this.unAuthorize())
      );
  }

  private loginByCookies$() {
    return this.#httpS.get<IProfile>('Auth/My')
      .pipe(
        tap(profile => this.authorize(profile)),
        catchError(() => {
          this.unAuthorize(false)
          return of(null);
        })
      );
  }

  private authorize(profile: IProfile) {
    this.#profileS.setProfile(profile);
    this.#isAuthorized.set(true);
    this.#authorizationChecked$.next(true);
  }

  private unAuthorize(updateState = true) {
    if (updateState) {
      this.#profileS.setProfile(null);
      this.#isAuthorized.set(false);
    }
    this.#authorizationChecked$.next(true);
  }

  private checkSavedAuthorization() {
    if (this.#managerSettingsS.savedManagerSettings().saveCredentials) {
      this.loginByCookies$().subscribe();
    } else {
      this.unAuthorize(false);
    }
  }
}
