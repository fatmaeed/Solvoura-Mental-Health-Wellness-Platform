import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { IDisplayServiceProvider } from '../../Models/ServiceProviderModels/idisplay-service-provider';
import { ServiceProviderService } from '../../Services/service-provider-service';
import { ServiceProviderCard } from "../ServiceProvider/service-provider-card/service-provider-card";
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NavBar } from "../../Layouts/nav-bar/nav-bar";

@Component({
  selector: 'app-all-service-providers',
  standalone: true,
  imports: [ServiceProviderCard, CommonModule, FormsModule, NavBar],
  templateUrl: './all-service-providers.html',
  styleUrl: './all-service-providers.css'
})
export class AllServiceProviders implements OnInit {

  searchQuery: string = '';
  selectedSpecialization: string = '';
  serviceProviders: IDisplayServiceProvider[] = [];
  filteredProviders: IDisplayServiceProvider[] = [];
  specializations: string[] = [];

  constructor(
    private providerService: ServiceProviderService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.providerService.getServiceProviders().subscribe({
      next: (data) => {
        this.serviceProviders = data;
        this.filteredProviders = [...this.serviceProviders];
        this.extractSpecializations();
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Error fetching service providers:', error);
      }
    });
  }
  resetFilters() {
    this.searchQuery = '';
    this.selectedSpecialization = '';
    this.filteredProviders = [...this.serviceProviders];
  }
  applyFilters(): void {
    const query = this.searchQuery.toLowerCase().trim();
    const specialization = this.selectedSpecialization;

    this.filteredProviders = this.serviceProviders.filter(provider => {
      const matchesName = provider.firstName.toLowerCase().includes(query)||provider.lastName.toLowerCase().includes(query);
      const matchesSpec = specialization ? provider.specialization === specialization : true;
      return matchesName && matchesSpec;
    });
  }

  extractSpecializations(): void {
    const specs = this.serviceProviders.map(p => p.specialization);
    this.specializations = Array.from(new Set(specs)).sort();
  }
}
