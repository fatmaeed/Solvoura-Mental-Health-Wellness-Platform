import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, FormArray } from '@angular/forms';
import { IRegisterServiceProvider } from '../../../../Models/Auth/IRegisterServiceProvider';
import { Router } from '@angular/router';

@Component({
  selector: 'app-sign-up-as-professional',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './sign-up-as-professional.html',
  styleUrls: ['./sign-up-as-professional.css']
})
export class SignUpAsProfessional {
  @Input() errorMessage = '';
  @Output() register = new EventEmitter<IRegisterServiceProvider>();

  signupForm: FormGroup;
  showPassword = false;
  showConfirmPassword = false;
  eyeIcon = '<i class="bi bi-eye"></i>';
  eyeFillIcon = '<i class="bi bi-eye-fill"></i>';

  // Previews
  userImagePreview: string | null = null;
  nationalImagePreview: string | null = null;
  userAndNationalImagePreview: string | null = null;
  certificateImagePreviews: string[] = [];

  constructor(private fb: FormBuilder, private router: Router) {

    this.signupForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required, Validators.pattern(/^(010|011|012|015)\d{8}$/)]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      address: ['', Validators.required],
      confirmPassword: ['', Validators.required],
      agreeToTerms: [false, Validators.requiredTrue],
      username: ['', Validators.required],
      gender: ['', Validators.required],
      dob: ['', Validators.required],
      nationalNumber: ['', [Validators.required, Validators.pattern(/^\d{14}$/)]],
      specialization: [0, [Validators.required, Validators.min(0), Validators.max(6)]],
      userImage: [null, Validators.required],
      nationalImage: [null, Validators.required],
      userAndNationalImage: [null, Validators.required],
      clinicLocation: ['', Validators.maxLength(70)],
      description: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(500)]],
      examinationType: [0, [Validators.required, Validators.min(0), Validators.max(3)]],
      experienceInYears: [0, [Validators.required, Validators.min(0), Validators.max(60)]],
      experienceDescription: ['', Validators.maxLength(500)],
      pricePerHour: [0, [Validators.required, Validators.min(0), Validators.max(10000)]],
      certificates: this.fb.array([])
    });
  }

  get certificates(): FormArray {
    return this.signupForm.get('certificates') as FormArray;
  }
 
  addCertificate(): void {
    const group = this.fb.group({
      image: [null, Validators.required],
      certificateName: ['', [Validators.required, Validators.maxLength(100)]],
      description: ['', Validators.maxLength(200)],
      issueDate: ['', Validators.required]
    });
    this.certificates.push(group);
    this.certificateImagePreviews.push('');
  }

  removeCertificate(index: number): void {
    this.certificates.removeAt(index);
    this.certificateImagePreviews.splice(index, 1);
  }

  removeCertificateImage(index: number): void {
    this.certificates.at(index).get('image')?.setValue(null);
    this.certificateImagePreviews[index] = '';
  }

  onCertificateFileChange(event: any, index: number): void {
    const file = event.target.files[0];
    if (file) {
      this.certificates.at(index).get('image')?.setValue(file);
      const reader = new FileReader();
      reader.onload = () => {
        this.certificateImagePreviews[index] = reader.result as string;
      };
      reader.readAsDataURL(file);
    }
  }

  onFileChange(event: any, controlName: string): void {
    const file = event.target.files[0];
    if (file) {
      this.signupForm.get(controlName)?.setValue(file);
      const reader = new FileReader();
      reader.onload = () => {
        switch (controlName) {
          case 'userImage':
            this.userImagePreview = reader.result as string;
            break;
          case 'nationalImage':
            this.nationalImagePreview = reader.result as string;
            break;
          case 'userAndNationalImage':
            this.userAndNationalImagePreview = reader.result as string;
            break;
        }
      };
      reader.readAsDataURL(file);
    }
  }

  removeImage(controlName: string): void {
    this.signupForm.get(controlName)?.setValue(null);
    switch (controlName) {
      case 'userImage':
        this.userImagePreview = null;
        break;
      case 'nationalImage':
        this.nationalImagePreview = null;
        break;
      case 'userAndNationalImage':
        this.userAndNationalImagePreview = null;
        break;
    }
  }

  togglePasswordVisibility(field: 'password' | 'confirmPassword'): void {
    if (field === 'password') {
      this.showPassword = !this.showPassword;
    } else {
      this.showConfirmPassword = !this.showConfirmPassword;
    }
  }

  private formatDateToString(dateInput: string): string {
    const date = new Date(dateInput);
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  onSubmit(): void {
    if (this.signupForm.valid) {
      const formValue = this.signupForm.value;
      const registerData: IRegisterServiceProvider = {
        FirstName: formValue.firstName,
        LastName: formValue.lastName,
        Address: formValue.address,
        Email: formValue.email,
        PhoneNumber: formValue.phone,
        Password: formValue.password,
        ConfirmPassword: formValue.confirmPassword,
        UserName: formValue.username,
        BirthDate: this.formatDateToString(formValue.dob),
        Gender: formValue.gender,
        NationalNumber: formValue.nationalNumber,
        Specialization: formValue.specialization,
        ClinicLocation: formValue.clinicLocation || '',
        Description: formValue.description,
        ExaminationType: formValue.examinationType,
        ExperienceInYears: formValue.experienceInYears,
        ExperienceDescription: formValue.experienceDescription || '',
        PricePerHour: formValue.pricePerHour,
        UserImage: formValue.userImage,
        NationalImage: formValue.nationalImage,
        UserAndNationalImage: formValue.userAndNationalImage,
        Certificates: formValue.certificates.map((cert: any) => ({
          image: cert.image,
          certificateName: cert.certificateName,
          description: cert.description,
          issueDate: new Date(cert.issueDate)
        }))
      };
      this.register.emit(registerData);
    } else {
      this.signupForm.markAllAsTouched();
    }
  }
}
