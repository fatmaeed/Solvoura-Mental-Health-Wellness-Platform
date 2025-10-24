import { Component, EventEmitter, Input, Output, OnChanges, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { TokenHandlerService } from './../../../Services/token-handler-service';
import { ClientService } from '../../../Services/client-service';

@Component({
  selector: 'app-edit-client-profile',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './edit-client-profile-component.html',
  styleUrls: ['./edit-client-profile-component.css']
})
export class EditClientProfileComponent implements OnChanges {

  @Input() userData: any;
  @Output() onClose = new EventEmitter<void>();
  @Output() onSave = new EventEmitter<any>();

  profileForm!: FormGroup;
  username: string|null;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private tokenService: TokenHandlerService,
    private service :ClientService
  ) {
    this.username = this.tokenService.UserName;
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['userData'] && this.userData) {
      this.initializeForm();
    }
  }

  initializeForm() {
    this.profileForm = this.fb.group({
       id: [this.userData?.id],
      firstName: [this.userData?.firstName || ''],
      lastName: [this.userData?.lastName || ''],
      email: [this.userData?.email || ''],
      phoneNumber: [this.userData?.phoneNumber || ''],
      address: [this.userData?.address || ''],
      gender: [this.userData?.gender || ''],
      birthDate: [this.userData?.birthDate || '']
    });
  }

  saveChanges() {
    if (this.profileForm.valid) {
    const request = this.profileForm.value;

    const updateDto = {
      firstName: request.firstName,
      lastName: request.lastName,
      email: request.email,
      phoneNumber: request.phoneNumber,
      address: request.address,
      gender: request.gender === 'Male' ? 0 : 1, 
      birthDate: request.birthDate ? request.birthDate.split('T')[0] : null,

      alternativePhoneNumber: null,
      userImagePath: null,
      isAnon: false,
      isVerified: true,
      historyIllness: null
    };

     this.service.updateClient(request.id, updateDto).subscribe({
      next: () => {
        this.closeModal();
       this.router.navigate(['/client-profile/client-info']);
      },
      error: (err) => {
        console.error('Error updating profile', err);
      }
    });
  }
  }

  closeModal() {
    this.onClose.emit();
  }

  changePassword() {
    this.router.navigate([`/${this.username}/profile/change-password`]);
  }
}
