import { Injectable } from '@angular/core';

import { environment } from '../../environments/environment';
import { IUserLikes } from '../Models/iuser-likes';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class UserLikeService {
      baseUrl: string = environment.apiBaseUrl + 'api/UserLikes';
      constructor(private http:HttpClient) {} 
      putUserLike(userlike:IUserLikes) {
       return  this.http.put(this.baseUrl, userlike)
      }
  
}
