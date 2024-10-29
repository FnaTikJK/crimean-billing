import {
  ChangeDetectionStrategy,
  Component,
  computed,
  ElementRef,
  inject,
  input,
  OnInit,
  signal,
  viewChild
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCard, MatCardContent, MatCardFooter, MatCardHeader, MatCardTitle } from '@angular/material/card';
import { TariffService } from '../../services/tariff.service';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { ITariff } from '../../models/ITariff';
import { tap } from 'rxjs';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { MatDivider } from '@angular/material/divider';
import { MatRipple } from '@angular/material/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ISearchTariffRequestDTO } from '../../DTO/request/ISearchTariffRequestDTO';

@Component({
  selector: 'app-tariff-list',
  standalone: true,
  imports: [CommonModule, MatCard, MatCardHeader, MatCardFooter, MatCardTitle, MatPaginator, MatProgressSpinner, MatDivider, MatRipple, RouterLink, MatCardContent],
  templateUrl: './tariff-list.component.html',
  styleUrl: './tariff-list.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class TariffListComponent implements OnInit {

  #tariffS = inject(TariffService);
  #router = inject(Router);
  #route = inject(ActivatedRoute);

  tariffIDToExclude = input<string>();

  protected tariffsSection = viewChild.required<ElementRef<HTMLElement>>('tariffsSection');

  private displayMode = computed(() => this.tariffIDToExclude() ? 'change' : 'add');

  protected allTariffs = signal<(ITariff | undefined)[]>([]);
  protected itemsLength = signal<number>(0);
  protected itemsOnPage = signal(0);
  protected pageIndex = signal(0);

  protected loading = signal(true);

  protected itemWidthPx = 170;
  protected itemHeightPx = 117;

  ngOnInit() {
    this.calcItemsPerPage();
    this.getItemsLength();
  }

  protected changePage(changePageEvent: PageEvent) {
    this.loading.set(true);
    this.pageIndex.set(changePageEvent.pageIndex);
    const firstItemIndexOnANewPage = changePageEvent.pageIndex * this.itemsOnPage();

    if (!this.allTariffs()[firstItemIndexOnANewPage]) {

      const searchParams: ISearchTariffRequestDTO = {
        skip: firstItemIndexOnANewPage,
        take: this.itemsOnPage(),
        excludedTemplateIds: this.tariffIDToExclude() ? [this.tariffIDToExclude()!] : undefined
      };

      this.#tariffS.getTariffs$(searchParams)
        .pipe(
          tap(() => this.loading.set(false))
        )
        .subscribe((tariffs) => {
          const allTariffsCopy = [...this.allTariffs()];
          for (let i = firstItemIndexOnANewPage, j = 0; j < tariffs.length; i++, j++) {
            allTariffsCopy[i] = tariffs[j]
          }
          this.allTariffs.set(allTariffsCopy);
        });
    } else {
      this.loading.set(false);
    }
  }

  protected navigateToTariffPage(tariffTemplateID: string) {
    if (this.displayMode() === 'add') {
      this.#router.navigate(['../', 'pick-tariff', tariffTemplateID], { relativeTo: this.#route });
    } else {
      this.#router.navigate(['../', 'compare-tariff', tariffTemplateID], { relativeTo: this.#route });
    }
  }

  private calcItemsPerPage() {
    const tariffListElement: HTMLElement = this.tariffsSection().nativeElement;
    const maxItemsInRow = Math.floor(tariffListElement.clientWidth / this.itemWidthPx);
    const maxItemsInColumn = Math.floor(tariffListElement.clientHeight / this.itemHeightPx);
    this.itemsOnPage.set(maxItemsInRow * maxItemsInColumn);
  }

  private getItemsLength() {
    this.#tariffS.getTariffLength$(this.tariffIDToExclude())
      .subscribe(tariffLength => {
        this.itemsLength.set(tariffLength);
        this.allTariffs.set(Array(tariffLength));
        this.changePage({length: tariffLength, pageIndex: 0, previousPageIndex: undefined, pageSize: this.itemsOnPage()});
      });
  }
}
