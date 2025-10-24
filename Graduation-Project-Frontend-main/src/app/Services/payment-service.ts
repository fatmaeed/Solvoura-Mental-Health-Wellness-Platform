import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { ICreatePayment } from '../Models/icreate-payment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {
  baseUrl:string = environment.apiBaseUrl + 'api/Payment';

  constructor(private http:HttpClient) {}

  createPayment(payment:ICreatePayment):Observable<{paymentId:number}> {
    return this.http.post<{paymentId:number}>(`${this.baseUrl}`,payment) ;
  }
}
