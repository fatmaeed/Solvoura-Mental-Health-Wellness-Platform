import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ServiceProviderService } from '../../../Services/service-provider-service';
import { TokenHandlerService } from '../../../Services/token-handler-service';
import { IDisplayServiceProvider } from '../../../Models/ServiceProviderModels/idisplay-service-provider';
import { AbstractControl, FormArray, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { CustomAlert } from "../../../Customs/custom-alert/custom-alert";


@Component({
  selector: 'app-edit-service-provider',
  imports: [ReactiveFormsModule, CommonModule, CustomAlert],
  templateUrl: './edit-service-provider.html',
  styleUrl: './edit-service-provider.css'
})

export class EditServiceProvider implements OnInit {
  providerId!: number | null;
  providerData!: IDisplayServiceProvider;
  updateForm!: FormGroup;
  errorMessage = '';

  constructor(private service: ServiceProviderService,private router :Router, private cdr: ChangeDetectorRef, private token: TokenHandlerService, private fb: FormBuilder) {
    this.updateForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      address: ['', Validators.required],

      userImage: [null],
      clinicLocation: ['', Validators.maxLength(70)],
      description: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(500)]],
      examinationType: [0, [Validators.required, Validators.min(0), Validators.max(3)]],
      experienceInYears: [0, [Validators.required, Validators.min(0), Validators.max(60)]],
      pricePerHour: [0, [Validators.required, Validators.min(0), Validators.max(10000)]],
      certificates: this.fb.array([])
    })
  }

  ngOnInit(): void {
    this.providerId = this.token.UserId;
    if (!this.providerId) return
    this.loadProvider(this.providerId);
  }

  specializationEnumMap: Record<string, number> = {
  Doctors: 0,
  Specialists: 1,
  SpeechTherapists: 2,
  LifeCoaches: 3,
  Therapists: 4
};
type:Record<string,number> ={
  Online :0,
  Offline:1,
  Both:2
}
minimumAgeValidator(minAge: number) {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;
    if (!value) return null;

    const birthDate = new Date(value);
    const today = new Date();

    let age = today.getFullYear() - birthDate.getFullYear();
    const monthDiff = today.getMonth() - birthDate.getMonth();

    if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthDate.getDate())) {
      age--;
    }

    return age >= minAge ? null : { minAge: { requiredAge: minAge, actualAge: age } };
  };
}
  loadProvider(id: number) {
    this.service.getServiceProviderById(id).subscribe({
      next: (data) => {
        this.providerData = data;
        this.updateForm.patchValue({
          firstName: data.firstName,
          lastName: data.lastName,
          address: data.address,

          clinicLocation: data.clinicLocation,
          description: data.description,
          examinationType: this.type[data.examinationType] ?? null,
          experienceInYears: data.numberOfExp,
          pricePerHour: data.pricePerHour,
        });
        this.cdr.detectChanges();
        console.log(this.providerData);
      },
      error: (err) => {
        console.log("Error load provider", err)
      }
    })
  }

  get certificates(): FormArray {
    return this.updateForm.get('certificates') as FormArray;
  }
  addCertificate(): void {
    const group = this.fb.group({
      image: [null, Validators.required],
      certificateName: ['', [Validators.required, Validators.maxLength(100)]],
      description: ['', Validators.maxLength(200)],
      issueDate: ['', Validators.required]
    });
    this.certificates.push(group);
  }

  removeCertificate(index: number): void {
    this.certificates.removeAt(index);
  }

  onFileChange(event: any, controlName: string): void {
    const file = event.target.files[0];
    if (file) {
      this.updateForm.get(controlName)?.setValue(file);
    }
  }
  onCertificateFileChange(event: any, index: number): void {
    const file = event.target.files[0];
    if (file) {
      this.certificates.at(index).get('image')?.setValue(file);
    }
  }
  private formatDateToString(dateInput: string): string {
    const date = new Date(dateInput);
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }
showSuccessDialog = false;

  onSubmit(): void {
    if (this.updateForm.valid) {
      const formValue = this.updateForm.value;
      const formData = new FormData();

      formData.append('Id', this.providerId!.toString());
      formData.append('FirstName', formValue.firstName);
      formData.append('LastName', formValue.lastName);
      formData.append('Address', formValue.address);

      formData.append('ClinicLocation', formValue.clinicLocation || '');
      formData.append('Description', formValue.description);
      formData.append('ExaminationType', formValue.examinationType.toString());
      formData.append('NumberOfExp', formValue.experienceInYears.toString());
      formData.append('PricePerHour', formValue.pricePerHour.toString());
      formData.append('Experience', formValue.experienceDescription ?? '');

      if (formValue.userImage) {
        formData.append('UserImage', formValue.userImage);
      }

      formValue.certificates.forEach((cert: any, index: number) => {
        formData.append(`Certificates[${index}].ServiceProviderId`, this.providerId!.toString());
        formData.append(`Certificates[${index}].CertificateName`, cert.certificateName);
        formData.append(`Certificates[${index}].Description`, cert.description ?? '');
        formData.append(`Certificates[${index}].IssueDate`, this.formatDateToString(cert.issueDate));
        formData.append(`Certificates[${index}].Image`, cert.image);
      });

      this.service.updateProvider(formData).subscribe({
        next: () => {
           this.showSuccessDialog = true;
           this.cdr.detectChanges();

           setTimeout(() => {
            window.location.href = '/service-provider-dashboard/display-sessions-to-sp';
          }, 2000);          },
        error: (err) => {
          alert("Error updating provider");
        }
      });

    } else {
      this.updateForm.markAllAsTouched();
    }
  }

}
