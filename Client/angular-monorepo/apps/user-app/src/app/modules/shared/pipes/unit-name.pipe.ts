import { Pipe, PipeTransform } from '@angular/core';
import { UnitType } from '../../subscriptions/submodules/service/models/UnitType';

@Pipe({
  name: 'unitName',
  standalone: true,
})
export class UnitNamePipe implements PipeTransform {
  transform(unitType: UnitType): string {
    switch (unitType) {
      case UnitType.Units:
        return 'Шт.';
      case UnitType.Gb:
        return 'Гб.';
      case UnitType.Mb:
        return 'Мб.';
    }
  }
}
