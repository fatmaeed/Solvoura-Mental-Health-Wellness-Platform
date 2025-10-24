import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SessionService {
  constructor(private httpClient : HttpClient) { }

  GetMeetingSession(id :number) : Observable<IMeetingSession>{
    return this.httpClient.get(`${environment.apiBaseUrl}api/Session/GetMeetingSession/${id}`) as Observable<IMeetingSession>;
  }
  
}
