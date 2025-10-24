import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AIService {
  private apiUrl = environment.apiBaseUrl + 'api/AI/ask';

  constructor(private http: HttpClient) {}

  getAIResponse(prompt: string): Observable<{ question: string; answer: string }> {
    return this.http.post<{ question: string; answer: string }>(
      this.apiUrl,
      JSON.stringify(prompt), 
      {
        headers: {
          'Content-Type': 'application/json'
        }
      }
    );
  }
}
