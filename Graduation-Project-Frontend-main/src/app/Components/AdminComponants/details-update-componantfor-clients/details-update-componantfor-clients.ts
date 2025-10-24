import { ActivatedRoute, Router } from '@angular/router';
import { environment } from '../../../../environments/environment';
  import { IDisplayClient } from './../../../Models/Client/IDisplayClient';
import { ChangeDetectorRef, Component, ViewChild } from '@angular/core';
import { ClientService } from '../../../Services/ClientService/clientService';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { SuccessToastComponent } from '../../../Customs/success-toast-component/success-toast-component';
import { ToastrService } from 'ngx-toastr';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-details-update-componantfor-clients',
  imports: [CommonModule, FormsModule,SuccessToastComponent],
  templateUrl: './details-update-componantfor-clients.html',
  styleUrl: './details-update-componantfor-clients.css'
})
export class DetailsUpdateComponantforClients {
  isEditMode = false;
  client!: IDisplayClient;
baseUrl = environment.apiBaseUrl;
@ViewChild(SuccessToastComponent) toastComponent!: SuccessToastComponent;

  constructor(
    private dialog: MatDialog,

    private route: ActivatedRoute,
    private clientService: ClientService,
    private cdr: ChangeDetectorRef,
    private router:Router
  ) {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    const navState = window.history.state;

    this.isEditMode = navState?.isEditMode ?? false;

    this.clientService.getClientById(id).subscribe((data) => {
      this.client = data;
      console.log(this.client);
      this.cdr.detectChanges();
    });
  }

  onSave() {
    const updatedClient = { ...this.client };

    const genderMap: { [key: string]: number } = {
      'Male': 0,
      'Female': 1
    };
    if (updatedClient.gender && typeof updatedClient.gender == 'string') {
      updatedClient.gender = genderMap[updatedClient.gender] as unknown as any;
    }

    // Format birthDate as "yyyy-MM-dd"
    if (updatedClient.birthDate) {
      const date = new Date(updatedClient.birthDate);
      updatedClient.birthDate = date.toISOString().split('T')[0];
    }

    console.log('Sending to server:', updatedClient);

    this.clientService.updateClient(updatedClient.id, updatedClient)
      .subscribe(() => {


              this.isEditMode = false;
        this.cdr.detectChanges();

                              this.router.navigate(['/admin-dashboard/all-clients']);

      });
  }

}
