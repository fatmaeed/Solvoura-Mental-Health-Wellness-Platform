import { ChangeDetectorRef, Component } from '@angular/core';
import { ServiceProviderService } from '../../../Services/ServiceProviderService/serviceProviderService';
import { IDisplayServiceProvider } from '../../../Models/ServiceProviderModels/idisplay-service-provider';
import { Router } from '@angular/router';
import { AdminSideBar } from "../../../Layouts/admin-side-bar/admin-side-bar";

@Component({
  selector: 'app-pending-service-providers',
  imports: [AdminSideBar],
  templateUrl: './pending-service-providers.html',
  styleUrl: './pending-service-providers.css'
})
export class PendingServiceProviders {
  pendingServiceProviders: IDisplayServiceProvider[] = [];
  serviceProvider: IDisplayServiceProvider | null = null;
  constructor(private serviceProviderService: ServiceProviderService, private cdr: ChangeDetectorRef, private router: Router) {
    this.serviceProviderService.getPendingServiceProviders().subscribe((data) => {
      this.pendingServiceProviders = data.filter(sp => sp.isApproved == false);
      console.log(this.pendingServiceProviders);
      this.cdr.detectChanges();
    });

  }
  getServiceProviderById(id: number) {
    this.serviceProviderService.getServiceProviderById(id).subscribe((data) => {
      this.serviceProvider = data;
      this.cdr.detectChanges();
      this.router.navigate(['admin-dashboard/service-provider-details', id]);
    });
  }
}
