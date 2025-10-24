import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { TokenHandlerService } from "../token-handler-service";
import { IDisplayServiceProvider } from "../../Models/ServiceProviderModels/idisplay-service-provider";
import { Observable } from "rxjs";
import { IDisplayClient } from "../../Models/Client/IDisplayClient";
import { IOurService } from "../../Models/OurService/iour-service";

@Injectable({
  providedIn: 'root'
})
export class ServiceProviderService {

  apiUrl: string =environment.apiBaseUrl + "api";

  constructor(private http:HttpClient, private tokenHandlerService: TokenHandlerService) { }

  getPendingServiceProviders(): Observable<IDisplayServiceProvider[]> {
    return this.http.get<IDisplayServiceProvider[]>(`${this.apiUrl}/ServiceProvider/getUnApprovedServiceProviders`);
  }

  getServiceProviders(): Observable<IDisplayServiceProvider[]> {
    return this.http.get<IDisplayServiceProvider[]>(`${this.apiUrl}/ServiceProvider`);
  }

  getServiceProviderById(id: number): Observable<IDisplayServiceProvider> {
    return this.http.get<IDisplayServiceProvider>(`${this.apiUrl}/ServiceProvider/${id}`);
  }

  getClients(): Observable<IDisplayClient[]> {
    return this.http.get<IDisplayClient[]>(`${this.apiUrl}/Clients`);
  }

  getOurServices(): Observable<IOurService[]> {
    return this.http.get<IOurService[]>(`${this.apiUrl}/OurServices`);
  }

  approveServiceProvider(id: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/ServiceProvider/ApproveServiceProvider/${id}`, {});
  }

  rejectServiceProvider(id: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/ServiceProvider/RejectServiceProvider/${id}`, {});
  }
  deleteServiceProvider(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/ServiceProvider/${id}`);
  }
  updateServiceProvider(id: number, serviceProvider: IDisplayServiceProvider): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/ServiceProvider/${id}`, serviceProvider);
  }
  getIncomingSession(id:number):Observable<IMeetingSession[]>{
    return this.http.get<IMeetingSession[]>(`${this.apiUrl}/ServiceProvider/Incoming/${id}`);
  }
}
