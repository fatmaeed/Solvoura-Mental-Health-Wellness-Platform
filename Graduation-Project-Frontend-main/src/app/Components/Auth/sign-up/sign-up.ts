import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { AccountService } from '../../../Services/Auth/account-service';
import { SignUpAsClient } from './sign-up-as-client/sign-up-as-client';
import { SignUpAsProfessional } from './sign-up-as-professional/sign-up-as-professional';
import { IRegisterClient } from '../../../Models/Auth/IRegisterClient';
import { IRegisterServiceProvider } from '../../../Models/Auth/IRegisterServiceProvider';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [CommonModule, SignUpAsClient, SignUpAsProfessional],
  templateUrl: './sign-up.html',
  styleUrls: ['./sign-up.css'],
})
export class SignUp {
  accountType: 'needSupport' | 'professional' = 'needSupport';

  errorMessage = '';

  constructor(
    private accountService: AccountService,
    private router: Router,
  ) {}

  toggleAccountType(type: 'needSupport' | 'professional'): void {
    this.accountType = type;
    this.errorMessage = '';
  }
  goToLogin(): void {
    this.router.navigate(['/login']);
  }

  private formatDateToString(dateInput: string): string {
    const date = new Date(dateInput);
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }
 goToHome() {
    this.router.navigate(['/']);
  }


  onRegister(formData: IRegisterClient | IRegisterServiceProvider): void {
    this.errorMessage = '';

    const apiType = this.accountType === 'needSupport' ? 'client' : 'provider';
    if( apiType == 'provider' ) {
      const form = new FormData();
      formData = formData as IRegisterServiceProvider
  form.append('FirstName', formData.FirstName);
  form.append('LastName', formData.LastName);
  form.append('UserName', formData.UserName);
  form.append('Email', formData.Email);
  form.append('PhoneNumber', formData.PhoneNumber);
  form.append('Password', formData.Password);
  form.append('ConfirmPassword', formData.ConfirmPassword);
  form.append('Gender', formData.Gender.toString());
  form.append('BirthDate', formData.BirthDate);
  form.append('Address', formData.Address);
  form.append('NationalNumber', formData.NationalNumber);
  form.append('Specialization', formData.Specialization.toString())
  form.append('UserImage', formData.UserImage);
  form.append('NationalImage', formData.NationalImage);
  form.append('UserAndNationalImage', formData.UserAndNationalImage);

  if (formData.ClinicLocation) {
    form.append('ClinicLocation', formData.ClinicLocation);
  }

  form.append('Description', formData.Description);
  form.append('ExaminationType', formData.ExaminationType.toString());
  form.append('ExperienceInYears', formData.ExperienceInYears.toString());

  if (formData.ExperienceDescription) {
    form.append('ExperienceDescription', formData.ExperienceDescription);
  }
  form.append('PricePerHour', formData.PricePerHour.toString());
  formData.Certificates.forEach((cert: any, index: number) => {
        form.append(`Certificates[${index}].CertificateName`, cert.certificateName);
        form.append(`Certificates[${index}].Description`, cert.description ?? '');
        form.append(`Certificates[${index}].IssueDate`, this.formatDateToString(cert.issueDate));
        form.append(`Certificates[${index}].Image`, cert.image);
      });


          this.accountService.registerUser(form, apiType).subscribe({
      next: () => {

        this.router.navigate(['/confirm-email'],  {
          queryParams: { email: (formData as any).email }
        });
      },
      error: (err) => {
        this.errorMessage = 'E-mail or username already in use.';

        if (typeof err === 'string') {
          this.errorMessage = 'E-mail or username already in use.';
        } else if (typeof err === 'object') {
          this.errorMessage = Object.values(err).flat().join('\n');
        } else {
          this.errorMessage = 'E-mail or username already in use.';
        }
      },
    });

    }else {
    this.accountService.registerUser(formData, apiType).subscribe({
      next: () => {

        this.router.navigate(['/confirm-email'],  {
          queryParams: { email: (formData as any).email }
        });
      },
      error: (err) => {
        this.errorMessage = 'E-mail or username already in use.';

        if (typeof err === 'string') {
          this.errorMessage = 'E-mail or username already in use.';
        } else if (typeof err === 'object') {
          this.errorMessage = Object.values(err).flat().join('\n');
        } else {
          this.errorMessage = 'E-mail or username already in use.';
        }
      },
    });
    }

    console.log(formData)

  }
}
