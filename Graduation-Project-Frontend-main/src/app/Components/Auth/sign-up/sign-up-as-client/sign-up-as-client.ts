import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { IRegisterClient } from '../../../../Models/Auth/IRegisterClient';

@Component({
  selector: 'app-sign-up-as-client',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './sign-up-as-client.html',
  styleUrls: ['./sign-up-as-client.css']
})
export class SignUpAsClient {
  @Input() errorMessage = '';
  @Output() register = new EventEmitter<IRegisterClient>();

  signupForm: FormGroup;
  showPassword = false;
  showConfirmPassword = false;

  userImagePreview: string | null = null;

  eyeIcon = '<i class="bi bi-eye"></i>';
  eyeFillIcon = '<i class="bi bi-eye-fill"></i>';

  constructor(private fb: FormBuilder) {
    this.signupForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required, Validators.pattern(/^(010|011|012|015)\d{8}$/)]],
      gender: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', Validators.required],
      agreeToTerms: [false, Validators.requiredTrue],
      username: ['', Validators.required],
      dob: ['', Validators.required],
      address: ['', Validators.required],
      illness: ['', Validators.required]
     // userImage: [null, Validators.required]
    });
  }

  togglePasswordVisibility(field: 'password' | 'confirmPassword'): void {
    if (field === 'password') {
      this.showPassword = !this.showPassword;
    } else {
      this.showConfirmPassword = !this.showConfirmPassword;
    }
  }

  onFileChange(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.signupForm.get('userImage')?.setValue(file);

      const reader = new FileReader();
      reader.onload = () => {
        this.userImagePreview = reader.result as string;
      };
      reader.readAsDataURL(file);
    }
  }

  removeImage(): void {
    this.signupForm.get('userImage')?.setValue(null);
    this.userImagePreview = null;
  }

  private formatDateToString(dateInput: string): string {
    const date = new Date(dateInput);
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  onSubmit(): void {
    console.log("fffff")
    if (this.signupForm.valid) {
      const formValue = this.signupForm.value;
      const registerData: IRegisterClient = {
        firstName: formValue.firstName,
        lastName: formValue.lastName,
        email: formValue.email,
        phoneNumber: formValue.phone,
        password: formValue.password,
        confirmPassword: formValue.confirmPassword,
        userName: formValue.username,
        birthDate: this.formatDateToString(formValue.dob),
        address: formValue.address,
        illnessesHistory: formValue.illness,
        gender: +formValue.gender,
        neededSpecilization: 1,
        //userImagePath: this.uploadedImagePath
      };

      this.register.emit(registerData);
    } else {
      this.signupForm.markAllAsTouched();
    }
  }
}
