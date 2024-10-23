import { Pipe, PipeTransform } from '@angular/core';
import { ServiceType } from '../../subscriptions/submodules/service/models/ServiceType';

@Pipe({
  name: 'serviceName',
  standalone: true,
})
export class ServiceNamePipe implements PipeTransform {
  transform(serviceType: ServiceType): string {
    switch (serviceType) {
      case ServiceType.Internet:
        return 'Интернет';
      case ServiceType.SMS:
        return 'SMS';
      case ServiceType.MMS:
        return 'MMS';
      case ServiceType.Calls:
        return 'Звонки';
    }
  }
}
