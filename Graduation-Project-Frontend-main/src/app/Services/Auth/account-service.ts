import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { IRegisterClient } from '../../Models/Auth/IRegisterClient';
import { IRegisterServiceProvider } from '../../Models/Auth/IRegisterServiceProvider';
import { environment } from '../../../environments/environment';
import { jwtDecode } from 'jwt-decode';
import { IJWTClaims } from '../../Models/ijwtclims';
import { ILoginUser } from '../../Models/Auth/ILoginUser';
import { NotificationSignalR } from '../notification-signal-r';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl: string = environment.apiBaseUrl + 'api/Account';

  constructor(private http: HttpClient, private notificationSignalR: NotificationSignalR) {}
  registerUser(
    user: any,
    accountType: 'client' | 'provider'
  ) {
    const url = `${this.baseUrl}/register/${accountType}`;
    const formData = this.buildFormData(user);
    if(accountType == 'client') {
          return this.http.post<{ message: string }>(`${url}`, formData).pipe(
      catchError((error: HttpErrorResponse) => {
        console.error(' Error from backend:', error);
        if (error.error && typeof error.error === 'object') {
          if (error.error.title) {
            return throwError(() => error.error.title);
          }
          if (error.error.errors) {
            const allErrors = Object.values(error.error.errors)
              .flat()
              .join(' ');
            return throwError(() => allErrors);
          }
        }
        if (typeof error.error === 'string') {
          return throwError(() => error.error);
        }

        if (error.status === 400) {
          return throwError(() => 'Bad Request: Please check the form.');
        }
        if (error.status === 401) {
          return throwError(() => 'Unauthorized: Please log in.');
        }
        return throwError(() => 'Unexpected error occurred.');
      })
    );
    }else {
      return this.http.post(`${url}` , user) ;
    }
  }


  private buildFormData(data: any): FormData {
    const formData = new FormData();

    for (const key in data) {
      if (data.hasOwnProperty(key)) {
        const value = data[key];

        if (value instanceof File) {
          formData.append(key, value, value.name);
        } else if (value !== null && value !== undefined) {
          formData.append(key, value.toString());
        }
      }
    }

    return formData;
  }
 

login(user:ILoginUser ) :Observable<{token : string , message : string}>{
 
  return this.http.post(`${this.baseUrl}/login`, user) as Observable<{token : string , message : string}>;
}

isLoggedIn(): boolean {
  return !!localStorage.getItem('token');
}
forgetPassword(email : string) : Observable<string> {
  return this.http.get(`${this.baseUrl}/forget-password?email=${email}`) as Observable<string>;
}

resetPassword(resetObject : {email : string | null, password : string , confirmPassword : string , token : string | null}) : Observable<{token : string , message : string}> {
  return this.http.put( `${this.baseUrl}/reset-password` , resetObject , {
    headers: {
      'Content-Type': 'application/json'
    }
  }) as Observable<{token : string , message : string}>;
}
changePassword(changePasswordObject : {userId : number ,oldPassword : string , newPassword : string , confirmPassword : string }) : Observable<{token : string , message : string}> {
  return this.http.put( `${this.baseUrl}/change-password` , changePasswordObject , {
    headers: {
      'Content-Type': 'application/json'
    }
  }) as Observable<{token : string , message : string}>;
}
logout ()  {
  this.notificationSignalR.stop();
  localStorage.removeItem('token');
  
}
getUserId(): string | null {
  return localStorage.getItem('userId');
}
getUserIdFromToken(): string | null {
  const token = localStorage.getItem('token');
  if (!token) return null;

  try {
    const decoded = jwtDecode<IJWTClaims>(token);
    return decoded.id;
  } catch (e) {
    console.error('Invalid token', e);
    return null;
  }
}
  confirmEmail(token :string , email : string) : Observable<{token : string , message : string }> {
    return this.http.get(`${this.baseUrl}/ConfirmEmail?token=${token}&email=${email}`) as Observable<{token : string , message : string }>;
  }

}