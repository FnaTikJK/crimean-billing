import { Injectable, signal } from '@angular/core';
import { ProfileRoleType } from '@angular-monorepo/shared-separate-entities';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {
  #profile = signal<IProfile | null>(null);
  profile = this.#profile.asReadonly();

  setProfile(profile: IProfile | null) {
    this.#profile.set(profile);
  }
}

export interface IProfile {
  id: string;
  role: ProfileRoleType;
}
