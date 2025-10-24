import { CommonModule } from '@angular/common';
import { Component, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
  import { ConfirmMail } from "../../confirm-mail/confirm-mail";
import { AccountService } from '../../../Services/Auth/account-service';

@Component({
  selector: 'app-forget-password',
  imports: [ReactiveFormsModule, CommonModule, ConfirmMail],
  templateUrl: './forget-password.html',
  styleUrl: './forget-password.css'
})
export class ForgetPassword {

  forgotPassForm: FormGroup;
  isSubmitting = false;
  isSent = false;
  errorValue : string | null = '';

  constructor(private fb: FormBuilder, private router: Router , private accountService: AccountService , private cdr: ChangeDetectorRef) {
    this.forgotPassForm = this.fb.group({
      Email: ['', [Validators.required, Validators.email]]
    });
  }
  goToLogin() {
    this.router.navigate(['/login']);
  }
  get Email() {
    return this.forgotPassForm.controls['Email'];
  }

  submit() {
    if (this.forgotPassForm.invalid) return;

    this.isSubmitting = true;
    this.accountService.forgetPassword(this.Email.value).subscribe({
    next: () => {
        this.isSubmitting = false;
        this.isSent = true;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.log(error);
        this.isSubmitting = false;
        this.errorValue = error.error.title;
        this.cdr.detectChanges();
      }
    });

  }

  goToHome() {
    this.router.navigate(['/']);
  }
}
