import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IClientSession } from '../Models/Client/iclient-session';
import { Observable } from 'rxjs';
import { IDisplayClient } from '../Models/Client/idisplay-client';
import { IClientRequest } from '../Models/Client/client-request';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ClientService {

  apiUrl: string =environment.apiBaseUrl + "api/Client";

  constructor(private http:HttpClient) { }


  getClientById(id: number): Observable<IDisplayClient> {
    return this.http.get<IDisplayClient>(`${this.apiUrl}/GetClientById/${id}`);
  }


  getClientProfile(clientId: number) : Observable<IClientSession[]>{
    return this.http.get<IClientSession[]>(`${this.apiUrl}/GetClientSessions/${clientId}`);
  }

 handleSessionForClient(request: IClientRequest) {
  return this.http.post(`${this.apiUrl}/HandleSessionForClient`, request);
}
 updateClient(id: number, data: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, data);
  }

}
