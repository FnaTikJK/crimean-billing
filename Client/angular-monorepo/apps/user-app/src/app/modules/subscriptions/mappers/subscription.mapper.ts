import { Injectable } from '@angular/core';
import { ISubscriptionsResponseDTO } from '../DTO/response/ISubscriptionsResponseDTO';
import { ISubscription } from '../models/ISubscription';

@Injectable({
  providedIn: 'root'
})
export class SubscriptionMapper {

  fromDTO(subscriptionDTO: ISubscriptionsResponseDTO): ISubscription {
    const paymentDate = subscriptionDTO.paymentDate;
    const [day, month, year] = paymentDate.split('.');
    const offset = new Date().getTimezoneOffset();
    return {
      ...subscriptionDTO,
      paymentDate: new Date(+new Date(+year, +month - 1, +day) + offset).toISOString()
    };
  }
}
