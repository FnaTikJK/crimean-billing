export enum ServiceType {
  Internet = 0,
  SMS = 1,
  MMS = 2,
  Calls = 3,
}

export const SERVICE_TYPES = [
  { name: 'Интернет', value: ServiceType.Internet },
  { name: 'SMS', value: ServiceType.SMS },
  { name: 'MMS', value: ServiceType.MMS },
  { name: 'Звонки', value: ServiceType.Calls },
]
