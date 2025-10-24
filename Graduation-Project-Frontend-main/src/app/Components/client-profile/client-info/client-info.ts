import { ChangePassword } from './../../change-password/change-password';
import { TokenHandlerService } from './../../../Services/token-handler-service';
import { AccountService } from './../../../Services/Auth/account-service';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { IDisplayClient } from '../../../Models/Client/IDisplayClient';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { environment } from '../../../../environments/environment';
import { ClientService } from '../../../Services/ClientService/clientService';
import { EditClientProfileComponent } from "../edit-client-profile-component/edit-client-profile-component";

@Component({
  selector: 'app-client-info',
  imports: [CommonModule, FormsModule, EditClientProfileComponent],
  templateUrl: './client-info.html',
  styleUrls: ['./client-info.css']
})
export class ClientInfo implements OnInit {
  userData: any;
  client!: IDisplayClient;
  apiUrl: string =environment.apiBaseUrl;
  showEditModal = false;

  constructor(
    private service: ClientService,
    private cdr: ChangeDetectorRef,
    private tokenHandler: TokenHandlerService,
    private router: Router,
    private accountService: AccountService
  ) {}

  ngOnInit(): void {
    const userId = this.tokenHandler.UserId;
    if (userId) {
      this.service.getClientById(userId).subscribe({
        next: (data) => {
          this.userData = data;
          this.cdr.detectChanges();
        },
        error: (err) => {
          console.error('Error fetching client:', err);
        }
      });
    } else {
      console.warn('User ID not found in token.');
    }
  }
  editProfile() {
    this.showEditModal = true;
  }

  closeEditModal() {
    this.showEditModal = false;
  }

  saveEditedProfile(updatedData: any) {
    this.service.updateClient(this.userData.id, updatedData).subscribe({
      next: () => {
        this.userData = { ...this.userData, ...updatedData };
        this.closeEditModal();
      },
      error: (err) => console.error('Error updating profile:', err)
    });
  }
  ChangePassword() {
    this.router.navigate(['/']);

  }
}
