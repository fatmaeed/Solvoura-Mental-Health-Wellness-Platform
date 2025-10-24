import { TokenHandlerService } from './../../../Services/token-handler-service';
import { Component, ChangeDetectorRef } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from '../../../Services/Auth/account-service';
import { ILoginUser } from '../../../Models/Auth/ILoginUser';
import { environment } from '../../../../environments/environment';
import { NotificationSignalR } from '../../../Services/notification-signal-r';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  showPassword = false;
  isLoading = false;
  role:string = '';
  loginModel!:ILoginUser

  eyeIcon = '<i class="bi bi-eye"></i>';
  eyeFillIcon = '<i class="bi bi-eye-fill"></i>';
  constructor(private accountService:AccountService ,
     private router: Router ,
      private changeDet: ChangeDetectorRef ,
      private tkHandler: TokenHandlerService,
      private notificationSignalR:NotificationSignalR
    ) { }
  loginForm = new FormGroup({
    userNameOrEmail: new FormControl('',[Validators.required]) ,
    password : new FormControl('' , [Validators.required]) ,
    rememberMe: new FormControl(false)
  })



  goToHome() {
    this.router.navigate(['/']);
  }



  get userNameOrEmail() {return this.loginForm.controls['userNameOrEmail']}
  get password() {return this.loginForm.controls['password']}
  get rememberMe() {return this.loginForm.controls['rememberMe']}

  isServerErrors:boolean = false ;
  errorMessage:string = ''

  onSubmit() {


     if(this.loginForm.status == "VALID") {
      this.isLoading = true;
      this.loginModel = {...this.loginForm.value} as ILoginUser
     this.accountService.login(this.loginModel).subscribe({
        next: (res) => {
          this.isLoading = false;

          localStorage.setItem('token', res.token);
           this.changeDet.detectChanges();

            this.notificationSignalR.initConnection(`${environment.apiBaseUrl}hubs/system` , () => {
              this.notificationSignalR.sendMessage('RegisterUser', this.tkHandler.UserId);
         });
       if(this.tkHandler.Role == 'CLIENT') {
          this.router.navigate(['/']);
        }else if(this.tkHandler.Role == 'SERVICEPROVIDER') {
          this.router.navigate(['/service-provider-dashboard']);
        }
        else if(this.tkHandler.Role == 'ADMIN') {
          this.router.navigate(['/admin-dashboard']);
        }

        },
        error: (err) => {
          this.isLoading = false;
          this.isServerErrors = true ;
          this.errorMessage = err.error.title
          this.changeDet.detectChanges();

        }
      }); ;
     }else {
        this.loginForm.markAllAsTouched()
    return;
     }

  }


  signInWithGoogle() {
    console.log('G');
  }

  signInWithApple() {
    console.log('A');
  }
}
