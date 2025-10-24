import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ICreateSessions, RepeatOption, SessionType } from '../../../Models/ServiceProviderModels/icreate-sessions';
import { CommonModule } from '@angular/common';
import { ServiceProviderService } from '../../../Services/service-provider-service';
import { Router, RouterLink } from '@angular/router';

import { TokenHandlerService } from '../../../Services/token-handler-service';

@Component({
  selector: 'app-create-sessions',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './create-sessions.html',
  styleUrl: './create-sessions.css'
})
export class CreateSessions {

  constructor(private serviceProviderService:ServiceProviderService , private router:Router, private tokenHandler:TokenHandlerService) { }
  sessionForm: FormGroup = new FormGroup({

      startDateTime : new FormControl(null, [Validators.required]) ,
      durationInMinutes: new FormControl(null, [Validators.required, Validators.min(1)]),
      type: new FormControl(SessionType.Online,[ Validators.required]),
      repeatedFor: new FormControl(RepeatOption.Single, [Validators.required ]),
      toDate:new FormControl(null)
  });

  get startDateTime() {return this.sessionForm.controls['startDateTime'];}
  get durationInMinutes() {return this.sessionForm.controls['durationInMinutes'];}
  get type() {return this.sessionForm.controls['type'];}
  get repeatedFor() {return this.sessionForm.controls['repeatedFor'];}
  get repeatedForValue() { return this.sessionForm.get('repeatedFor')?.value;}
  get toDate() { return this.sessionForm.controls['toDate'];}



  Type = SessionType;
  Repeat = RepeatOption;


typeOptions = Object.entries(SessionType)
  .filter(([key, value]) => typeof value === 'number')
  .map(([key, value]) => ({ label: key, value }));


repeatOptions = Object.entries(RepeatOption)
  .filter(([key, value]) => typeof value === 'number')
  .map(([key, value]) => ({ label: key, value }));


  submit() {
    if (this.sessionForm.status === 'VALID') {
     if(typeof this.sessionForm.value.type == 'number') {
        this.sessionForm.value.type = SessionType.Online.toString();
      }
      const session: ICreateSessions = this.sessionForm.value;
      session.serviceProviderId =  this.tokenHandler.UserId!;
      console.log('Session Data:', session);
      this.serviceProviderService.createSession(session).subscribe({
        next: (response) => {
          console.log('Session created successfully:', response);
          //this.router.navigate(['/service-provider-dashboard/display-sessions-to-sp']);
          window.location.replace('/service-provider-dashboard/display-sessions-to-sp')
         },
        error: (error) => { console.error('Error creating session:', error); }
      });
    }else{
      this.sessionForm.markAllAsTouched();
    }
  }


}
