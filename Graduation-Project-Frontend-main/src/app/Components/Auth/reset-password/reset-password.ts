import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component } from '@angular/core';
import {
  AbstractControl,

  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
  ValidationErrors
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
  import { MatSnackBar } from '@angular/material/snack-bar';
import { AccountService } from '../../../Services/Auth/account-service';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './reset-password.html',
  styleUrls: ['./reset-password.css']
})
export class ResetPassword {
  resetForm: FormGroup;
  showPassword = false;
  showConfirm = false;

  constructor(private fb: FormBuilder, private router: Router ,
    private crd: ChangeDetectorRef ,
    private activatedRoute: ActivatedRoute ,
    private snackBar: MatSnackBar,
       private accountService: AccountService) {
    this.resetForm = this.fb.group(
      {
        newPassword: ['', [Validators.required, Validators.minLength(8)]],
        confirmPassword: ['', Validators.required ]
      },
      { validators: this.passwordMatch }
    );
  }
  error : string | null = '';
  passwordMatch = (control: AbstractControl): ValidationErrors | null => {
    const group = control as FormGroup;
    const password = group.get('newPassword')?.value;
    const confirm = group.get('confirmPassword')?.value;
    return password === confirm ? null : { mismatch: true };
  };

  get newPassword() {
    return this.resetForm.get('newPassword')!;
  }

  get confirmPassword() {
    return this.resetForm.get('confirmPassword')!;
  }

  togglePassword() {
    this.showPassword = !this.showPassword;
  }

  toggleConfirm() {
    this.showConfirm = !this.showConfirm;
  }

  goToHome() {
    this.router.navigate(['/']);
  }

  onSubmit() {
    if (!this.resetForm.valid) return;
    let params = this.activatedRoute.snapshot.queryParamMap;
   let reset = { email : params.get('email') , token : params.get('token') , password : this.resetForm.value.newPassword  as string, confirmPassword : this.resetForm.value.confirmPassword as string };
   this.accountService.resetPassword(reset).subscribe({
      next: (data) => {
        localStorage.setItem('token', data.token);
        this.snackBar.open('Password reset successfully', 'Action', {
          duration: 5000,
          verticalPosition: 'top',
          horizontalPosition: 'right'

        });
        this.goToHome();
      }
      , error: (error) => {
          this.error = error.error.title;
          this.crd.detectChanges();
      }
    })

  }
}
