import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { IDisplayClient } from "../../Models/Client/IDisplayClient";
import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";



@Injectable({
  providedIn: 'root'
})

export class ClientService{
  private baseUrl:string = environment.apiBaseUrl + 'api/Client';
  constructor(private http:HttpClient){}

  getAllClients():Observable<IDisplayClient[]>{
    return this.http.get<IDisplayClient[]>(`${this.baseUrl}`);
  }
  deleteClient(id:number):Observable<void>{
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
  updateClient(id:number,client:IDisplayClient):Observable<void>{
    return this.http.put<void>(`${this.baseUrl}/${id}`,client);
  }

  getClientById(id:number):Observable<IDisplayClient>{
    return this.http.get<IDisplayClient>(`${this.baseUrl}/GetClientById/${id}`);
  }
}
