import { ChangeDetectorRef, Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { environment } from "../../../environments/environment";
import { IOurService } from "../../Models/OurService/iour-service";

@Injectable({
  providedIn: 'root'
})
export class OurServiceServices {
  private baseUrl:string = environment.apiBaseUrl + 'api/OurService';
  constructor(private http:HttpClient){}

  getAllOurServices():Observable<IOurService[]>{
    return this.http.get<IOurService[]>(`${this.baseUrl}`);
  }

  addOurService(ourService: IOurService): Observable<IOurService> {
    return this.http.post<IOurService>(`${this.baseUrl}`, ourService, {
      headers: { 'Content-Type': 'application/json' }
    });
  }

  getOurServiceById(id:number):Observable<IOurService>{
    return this.http.get<IOurService>(`${this.baseUrl}/${id}`);
  }

  updateOurService(id:number,ourService:IOurService):Observable<IOurService>{
    return this.http.put<IOurService>(`${this.baseUrl}/${id}`,ourService);
  }
  deleteOurService(id:number):Observable<void>{
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
