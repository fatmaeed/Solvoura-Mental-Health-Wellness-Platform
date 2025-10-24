import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IResrvationRequest } from '../Models/Reservation/iresrvation-request';
import { ISessionForReserv } from '../Models/Session/isession-for-reserv';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ReservationService {
  
  apiUrl: string = `${environment.apiBaseUrl}api/Reservation`;
   constructor(private httpClient : HttpClient) { }
  createReservation(request: IResrvationRequest): Observable<IResrvationRequest> {
    return this.httpClient.post<IResrvationRequest>(this.apiUrl, request);
  }

  //   getFreeSessions(serviceProviderId: number, status: string, count?: number): Observable<ISessionForReserv[]> {
  //   let url = `${this.apiUrl}/GetFreeSessions?serviceProviderId=${serviceProviderId}&status=${status}`;
  //   if (count) {
  //     url += `&count=${count}`;
  //   }
  //   return this.httpClient.get<ISessionForReserv[]>(url);    
  // }
  getFreeSessions(serviceProviderId: number, status: string,startDate?:string,endDate?:string,duration?:string): Observable<ISessionForReserv[]> {
    let url = `${this.apiUrl}/GetFreeSessions?serviceProviderId=${serviceProviderId}&status=${status}`;
   
    if (startDate) {
      url += `&startDate=${startDate}`;
    }
    if (endDate) {
      url += `&endDate=${endDate}`;
    }
    if (duration) {
      url += `&duration=${duration}`;
    }
    return this.httpClient.get<ISessionForReserv[]>(url);
  }
  
}