import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { INotification, INotificationUpdate } from '../Models/Notification/INotificationModel';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
   apiUrl: string =environment.apiBaseUrl + "api/Notification";

   constructor(private http :HttpClient){}

   getNotificationForUser(userId:number): Observable<INotification[]>{
    return this.http.get<INotification[]>(`${this.apiUrl}/GetNotiForUser/${userId}`);
   }
   getById(id:number):Observable<INotification>{
     return this.http.get<INotification>(`${this.apiUrl}/${id}`);
   }
    updateNoti(request: INotificationUpdate): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}`, request);
  }
  getUnReadNoti(id:number): Observable<INotification[]>{
return this.http.get<INotification[]>(`${this.apiUrl}/unread/${id}`);
  }
}
