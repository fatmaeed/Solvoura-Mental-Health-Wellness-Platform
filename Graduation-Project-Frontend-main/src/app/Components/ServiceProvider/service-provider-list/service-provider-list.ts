import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ServiceProviderService } from '../../../Services/service-provider-service';
import { IDisplayServiceProvider } from '../../../Models/ServiceProviderModels/idisplay-service-provider';
import { ServiceProviderCard } from "../service-provider-card/service-provider-card";
import { Router } from '@angular/router';

@Component({
  selector: 'app-service-provider-list',
  imports: [ServiceProviderCard],
  templateUrl: './service-provider-list.html',
  styleUrl: './service-provider-list.css'
})
export class ServiceProviderList implements OnInit {

  serviceProviders: IDisplayServiceProvider[] = [];
  constructor(private providerService:ServiceProviderService  , private cdr:ChangeDetectorRef , private router:Router) {}
  seeAllProviders() {
    this.router.navigate(['/all-service-providers']);
    }

  ngOnInit(): void {
    this.providerService.getServiceProviders().subscribe({
      next: (data) => {
        this.serviceProviders = data;
        console.log(this.serviceProviders);
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Error fetching service providers:', error);
      }
  });

  }
}
