import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Post } from '../../Models/Post/PostModel';
import { environment } from '../../../environments/environment';
import { CreatePost } from '../../Models/Post/CreatePost';

@Injectable({ providedIn: 'root' })
export class PostService {
  private baseUrl = environment.apiBaseUrl + 'api/Post';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Post[]> {
    return this.http.get<Post[]>(this.baseUrl);
  }

  create(post: CreatePost): Observable<any> {
    return this.http.post(this.baseUrl, post);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }

  update(post: Post): Observable<any> {
    return this.http.put(this.baseUrl, post);
  }
}
