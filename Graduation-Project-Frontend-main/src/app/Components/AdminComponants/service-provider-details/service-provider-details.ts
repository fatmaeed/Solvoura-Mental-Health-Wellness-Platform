import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { IDisplayServiceProvider } from '../../../Models/ServiceProviderModels/idisplay-service-provider';
import { ServiceProviderService } from '../../../Services/ServiceProviderService/serviceProviderService';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AdminSideBar } from "../../../Layouts/admin-side-bar/admin-side-bar";
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-service-provider-details',
  imports: [CommonModule],
  templateUrl: './service-provider-details.html',
  styleUrl: './service-provider-details.css'
})
export class ServiceProviderDetails implements OnInit {
  serviceProvider!: IDisplayServiceProvider;
  baseUrl = environment.apiBaseUrl;
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private serviceProviderService: ServiceProviderService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    console.log('Route param ID:', idParam);

    const id = Number(idParam);
    if (id) {
      this.serviceProviderService.getServiceProviderById(id).subscribe((data) => {
        this.serviceProvider = data;
        console.log('Fetched Service Provider:', data);
        this.cdr.detectChanges();
      });
    }
  }
  approve() {
    this.serviceProviderService.approveServiceProvider(this.serviceProvider.id).subscribe(() => {
      this.router.navigate(['/admin-dashboard/pending-service-providers']);
    });
  }
  reject() {
    this.serviceProviderService.rejectServiceProvider(this.serviceProvider.id).subscribe(() => {
      this.router.navigate(['/admin-dashboard/pending-service-providers']);
    });
  }
}
