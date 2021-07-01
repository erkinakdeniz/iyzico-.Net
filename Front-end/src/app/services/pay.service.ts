import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { PaymentCard } from '../models/paymentCard';
@Injectable({
  providedIn: 'root'
})
export class PayService {

  constructor(private httpClient:HttpClient) { }
  addPay(paymentCard:PaymentCard):Observable<any>{
    let newPath=environment.apiUrl+"Iyzico/pay"
    return this.httpClient.post<PaymentCard>(newPath,paymentCard);
  }

}
