import { TokenHandlerService } from './../../Services/token-handler-service';
import { ChangeDetectorRef, Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AccountService } from '../../Services/Auth/account-service';

@Component({
  selector: 'app-change-password',
  imports: [ReactiveFormsModule , CommonModule],
  templateUrl: './change-password.html',
  styleUrl: './change-password.css'
})
export class ChangePassword {
  resetForm: FormGroup;
  showPassword = false;
  showConfirm = false;

  constructor(private fb: FormBuilder, private router: Router ,
    private crd: ChangeDetectorRef ,
    private tokenHandler: TokenHandlerService ,
    private snackBar: MatSnackBar,
       private accountService: AccountService) {
    this.resetForm = this.fb.group(
      {
        oldPassword: ['', [Validators.required, Validators.minLength(8)]],
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

  get oldPassword() {
    return this.resetForm.get('oldPassword')!;
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
   let change =
   { userId :this.tokenHandler.UserId as number ,
     oldPassword : this.resetForm.value.oldPassword as string ,
      newPassword : this.resetForm.value.newPassword  as string,
      confirmPassword : this.resetForm.value.confirmPassword as string };
console.log(change);
   this.accountService.changePassword(change).subscribe({
      next: (data) => {
        localStorage.setItem('token', data.token);
        this.snackBar.open('Password Changed successfully', 'Action', {
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
