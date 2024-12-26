import { ServiceType } from '../../subscriptions/submodules/service/models/ServiceType';

export const serviceTypeToIconName: Record<ServiceType, string> = {
  [ServiceType.Internet]: 'wifi',
  [ServiceType.SMS]: 'sms',
  [ServiceType.MMS]: 'mms',
  [ServiceType.Calls]: 'call'
}
