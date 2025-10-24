import { ChangeDetectorRef, Component } from '@angular/core';
import { IDisplayServiceProvider } from '../../../Models/ServiceProviderModels/idisplay-service-provider';
import { ServiceProviderService } from '../../../Services/ServiceProviderService/serviceProviderService';
import { AdminSideBar } from "../../../Layouts/admin-side-bar/admin-side-bar";
import { Router } from '@angular/router';

@Component({
  selector: 'app-all-service-providers',
  imports: [AdminSideBar],
  templateUrl: './all-service-providers.html',
  styleUrl: './all-service-providers.css'
})
export class AllServiceProvidersForAdmin {
  serviceProvider!: IDisplayServiceProvider;

  serviceProviders: IDisplayServiceProvider[] = [];
  constructor(private serviceProviderService: ServiceProviderService, private cdr: ChangeDetectorRef, private router: Router) {
    this.serviceProviderService.getServiceProviders().subscribe((data) => {
      this.serviceProviders = data;
      this.cdr.detectChanges();
      console.log(this.serviceProviders);
    });
  }
    getServiceProviderById(id: number) {
      this.serviceProviderService.getServiceProviderById(id).subscribe((data) => {
        this.serviceProvider = data;
        this.cdr.detectChanges();
        console.log(this.serviceProvider);
    });
  }
goToDetails(id: number) {

  this.router.navigate(['/admin-dashboard/details-update-componant', id], {
    state: { isEditMode: false }
  });
}

goToUpdate(id: number) {
  this.router.navigate(['/admin-dashboard/details-update-componant', id], {
    state: { isEditMode: true }
  });
}

  deleteServiceProvider(id: number) {
    this.serviceProviderService.deleteServiceProvider(id).subscribe((data) => {
      this.cdr.detectChanges();
      console.log(this.serviceProvider);
      window.location.reload();
    });
  }
}
