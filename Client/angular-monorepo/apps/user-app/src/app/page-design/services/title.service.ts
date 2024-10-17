import { inject, Injectable, signal } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { filter, switchMap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TitleService {

  #route = inject(ActivatedRoute);
  #router = inject(Router);

  #title = signal<string | null>(null);
  title = this.#title.asReadonly();

  constructor() {
    this.listenForRouteTitleChange();
  }

  get leaf(): ActivatedRoute {
    let leaf = this.#route;
    while (leaf.firstChild) {
      leaf = leaf.firstChild;
    }
    return leaf;
  }

  private listenForRouteTitleChange() {
    this.#router.events.pipe(
      filter((evt) => evt instanceof NavigationEnd),
      switchMap(() => this.leaf.title)
    ).subscribe(title => this.#title.set(title as string));
  }
}
