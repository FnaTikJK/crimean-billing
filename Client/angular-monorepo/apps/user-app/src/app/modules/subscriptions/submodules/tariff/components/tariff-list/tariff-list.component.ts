import { ChangeDetectionStrategy, Component, ElementRef, inject, input, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCard, MatCardFooter, MatCardHeader, MatCardTitle } from '@angular/material/card';
import { TariffService } from '../../services/tariff.service';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { ITariff } from '../../models/ITariff';
import { tap } from 'rxjs';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { MatDivider } from '@angular/material/divider';
import { MatRipple } from '@angular/material/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-tariff-list',
  standalone: true,
  imports: [CommonModule, MatCard, MatCardHeader, MatCardFooter, MatCardTitle, MatPaginator, MatProgressSpinner, MatDivider, MatRipple, RouterLink],
  templateUrl: './tariff-list.component.html',
  styleUrl: './tariff-list.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class TariffListComponent implements OnInit {

  #elementRef = inject(ElementRef);

  tariffIDToExclude = input<string>();
  #tariffS = inject(TariffService);

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
    const firstItemIndexOnANewPage = changePageEvent.pageIndex * this.itemsOnPage();

    if (!this.allTariffs()[firstItemIndexOnANewPage]) {
      this.#tariffS.getTariffs$({ skip: firstItemIndexOnANewPage, take: this.itemsOnPage() })
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

  private calcItemsPerPage() {
    const tariffListElement: HTMLElement = this.#elementRef.nativeElement;
    const maxItemsInRow = tariffListElement.clientWidth / this.itemWidthPx;
    const maxItemsInColumn = tariffListElement.clientHeight / this.itemHeightPx;
    this.itemsOnPage.set(Math.floor(maxItemsInRow * maxItemsInColumn));
  }

  private getItemsLength() {
    this.#tariffS.getTariffLength$()
      .subscribe(tariffLength => {
        this.itemsLength.set(tariffLength);
        this.allTariffs.set(Array(tariffLength));
        this.changePage({length: tariffLength, pageIndex: 0, previousPageIndex: undefined, pageSize: this.itemsOnPage()});
      });
  }
}
