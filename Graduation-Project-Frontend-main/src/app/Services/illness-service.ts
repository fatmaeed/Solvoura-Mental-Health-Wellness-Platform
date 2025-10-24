import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class IllnessService {
  baseUrl: string = environment.apiBaseUrl + 'api/Illness';

  constructor(private httpClient : HttpClient) { }

  getIllnesses() : Observable<IDisplayIllnessModel[]>{
    return this.httpClient.get(this.baseUrl) as Observable<IDisplayIllnessModel[]>;
  }

}
