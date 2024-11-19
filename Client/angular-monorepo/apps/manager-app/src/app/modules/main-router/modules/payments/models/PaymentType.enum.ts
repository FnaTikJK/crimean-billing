
export enum PaymentType {
  Add,
  Spend
}

export const PAYMENT_TYPES = [
  { name: "Все", value: undefined },
  { name: "Пополнение", value: PaymentType.Add },
  { name: "Расходы", value: PaymentType.Spend },
]
