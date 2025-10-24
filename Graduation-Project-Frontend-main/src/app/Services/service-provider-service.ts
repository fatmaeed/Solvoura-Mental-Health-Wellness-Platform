import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IDisplayServiceProvider } from '../Models/ServiceProviderModels/idisplay-service-provider';
import { Observable } from 'rxjs';
import { ICreateSessions, IEditSessions } from '../Models/ServiceProviderModels/icreate-sessions';
import { IDisplaySessionsToSP } from '../Models/ServiceProviderModels/idisplay-sessions-to-sp';
import { environment } from '../../environments/environment';
import { IdisplaySessionWithStatus } from '../Models/ServiceProviderModels/idisplay-session-with-status';
import { IProviderRequest } from '../Models/ServiceProviderModels/iprovider-request';
import { IEditServiceProvider } from '../Models/ServiceProviderModels/iedit-service-provider';


@Injectable({
  providedIn: 'root'
})
export class ServiceProviderService {

   baseUrl:string = environment.apiBaseUrl + 'api/ServiceProvider';
   baseUrlSession:string = environment.apiBaseUrl + 'api/Session';
   constructor(private http:HttpClient) { }

    getServiceProviders() : Observable<IDisplayServiceProvider[]> {
      return this.http.get<IDisplayServiceProvider[]>(`${this.baseUrl}`);
    }
    getServiceProviderById(id: number): Observable<IDisplayServiceProvider> {
      return this.http.get<IDisplayServiceProvider>(`${this.baseUrl}/${id}`);
    }

    createSession(session: ICreateSessions): Observable<any> {
      return this.http.post(`${this.baseUrl}/CreateSessions`, session);
    }
    getSessionsToSP(serviceProviderId:number): Observable<IDisplaySessionsToSP[]> {
      return this.http.get<IDisplaySessionsToSP[]>(`${this.baseUrl}/GetSessionsForServiceProvider/${serviceProviderId}`);
    }

    getSessionForClientRequest(serviceProviderId:number):Observable<IdisplaySessionWithStatus[]>{
      return this.http.get<IdisplaySessionWithStatus[]>(`${this.baseUrl}/GetSessionByStatus/${serviceProviderId}`)
    }

   handleDecideOnSession(request:IProviderRequest){
    return this.http.post(`${this.baseUrl}/HandleDecideOnSession`, request)
   }
   deleteSession(id: number) {
  return this.http.delete(`${this.baseUrlSession}/${id}`);
}

  editSession(request: IEditSessions) {
  return this.http.put(`${this.baseUrlSession}/EditSession`, request);
}
getSessionById(id: number) {
  return this.http.get(`${this.baseUrlSession}/${id}`);
}
// updateProvider(request: IEditServiceProvider): Observable<any> {
//   const formData = new FormData();

//   formData.append('id', request.id.toString());
//   formData.append('firstName', request.firstName);
//   formData.append('lastName', request.lastName);
//   formData.append('address', request.address);
//   formData.append('gender', request.gender.toString());
//   formData.append('birthDate', request.birthDate); // "yyyy-MM-dd"
//   formData.append('specialization', request.specialization.toString());
//   formData.append('clinicLocation', request.clinicLocation ?? '');
//   formData.append('description', request.description);
//   formData.append('examinationType', request.examinationType.toString());
//   formData.append('pricePerHour', request.pricePerHour.toString());
//   formData.append('numberOfExp', request.numberOfExp.toString());
//   formData.append('experience', request.experience ?? '');

//   if (request.userImage) {
//     formData.append('userImage', request.userImage);
//   }

//   request.certificates.forEach((cert, index) => {
//     formData.append(`certificates[${index}].serviceProviderId`, cert.serviceProviderId.toString());
//     formData.append(`certificates[${index}].certificateName`, cert.certificateName);
//     formData.append(`certificates[${index}].description`, cert.description ?? '');
//     formData.append(`certificates[${index}].issueDate`, cert.issueDate); // yyyy-MM-dd
//     formData.append(`certificates[${index}].image`, cert.image);
//   });

//   return this.http.put(`${this.baseUrl}/Editprovider`, formData);
// }

updateProvider(formData: FormData): Observable<any> {
  return this.http.put(`${this.baseUrl}/Editprovider`, formData);
}

}
