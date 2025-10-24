import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ICreateFeedBack } from '../Models/FeedBack/icreate-feed-back';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FeedBackService {

    baseUrl: string = environment.apiBaseUrl + 'api/FeedBack';
  

  constructor(private http:HttpClient) {} 

  createFeedBack(FeedBack:ICreateFeedBack ) {
   return this.http.post(this.baseUrl,FeedBack);
  }
  
}
