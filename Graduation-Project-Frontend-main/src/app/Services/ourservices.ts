import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { IOurService } from '../Models/OurService/iour-service';

@Injectable({
  providedIn: 'root'
})
export class Ourservices {

  apiUrl: string = environment.apiBaseUrl + 'api/OurService';
  constructor(private http :HttpClient ) { }
 gitAllServices(): Observable<IOurService []>{
    return this.http.get<IOurService[]>(this.apiUrl);
  }
  getServiceById(id: number): Observable<IOurService> {
    return this.http.get<IOurService>(`${this.apiUrl}/${id}`);
  }
  addService(service: IOurService): Observable<IOurService> {
    return this.http.post<IOurService>(this.apiUrl, service);
  }
  updateService(id: number, service: IOurService): Observable<IOurService> {
    return this.http.put<IOurService>(`${this.apiUrl}/${id}`, service);
  }
  deleteService(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

}
