import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CreateComment } from '../../Models/Comment/CreateComment';
import { CommentModel } from '../../Models/Comment/CommentModel';

@Injectable({ providedIn: 'root' })
export class CommentService {
  private baseUrl = environment.apiBaseUrl + 'api/Comment';

  constructor(private http: HttpClient) {}

  getByPost(postId: number): Observable<CommentModel[]> {
    return this.http.get<CommentModel[]>(`${this.baseUrl}/postId?postId=${postId}`);
  }

  create(comment: CreateComment): Observable<CommentModel> {
    return this.http.post<CommentModel>(this.baseUrl, comment);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }

  update(comment: CommentModel): Observable<any> {
    return this.http.put(this.baseUrl, comment);
  }
}
