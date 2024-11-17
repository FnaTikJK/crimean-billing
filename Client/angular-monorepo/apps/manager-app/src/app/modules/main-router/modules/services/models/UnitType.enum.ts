export enum UnitType {
  Units = 0,
  Mb = 1,
  Gb = 2,
}

export const UNIT_TYPES = [
  { name: 'Штуки', value: UnitType.Units },
  { name: 'Гигабайты', value: UnitType.Mb },
  { name: 'Мегабайты', value: UnitType.Gb },
]
