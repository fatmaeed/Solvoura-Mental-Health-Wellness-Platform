import { ChangeDetectorRef, Component, Input } from '@angular/core';
import { IDisplayServiceProvider } from '../../../Models/ServiceProviderModels/idisplay-service-provider';
import { ServiceProviderService } from '../../../Services/ServiceProviderService/serviceProviderService';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from '../../../../environments/environment';
import { AdminSideBar } from "../../../Layouts/admin-side-bar/admin-side-bar";

@Component({
  selector: 'app-details-update-componant',
  imports: [CommonModule, FormsModule, AdminSideBar],
  templateUrl: './details-update-componant.html',
  styleUrl: './details-update-componant.css'
})
export class DetailsUpdateComponant {
  serviceProvider!: IDisplayServiceProvider;
  isEditMode = false;
baseUrl = environment.apiBaseUrl;

  constructor(
    private route: ActivatedRoute,
    private serviceProviderService: ServiceProviderService,
    private cdr: ChangeDetectorRef,
    private router: Router
  ) {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    const navState = window.history.state;

    this.isEditMode = navState?.isEditMode ?? false;

    this.serviceProviderService.getServiceProviderById(id).subscribe((data) => {
      this.serviceProvider = data;
      this.cdr.detectChanges();
    });
  }

  onSave() {
    this.serviceProviderService
      .updateServiceProvider(this.serviceProvider.id, this.serviceProvider)
      .subscribe(() => {
        alert('Service provider updated successfully');
        this.isEditMode = false;
        this.cdr.detectChanges();
                      this.router.navigate(['/admin-dashboard/all-service-providers-for-admin']);


      });
  }
}
