import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { IOurService } from '../../../Models/OurService/iour-service';
import { CommonModule } from '@angular/common';
import { FeatureCardComponent } from "../../feature-card-componant/feature-card-componant";
import { Ourservices } from '../../../Services/ourservices';
import { environment } from '../../../../environments/environment';


@Component({
  selector: 'app-display-services',
  imports: [CommonModule, FeatureCardComponent],
  templateUrl: './display-services.html',
  styleUrl: './display-services.css'
})
export class DisplayServices implements OnInit {
  Services: IOurService[] =[] ;
  apiurl:string = environment.apiBaseUrl;

  constructor(private ourServices: Ourservices,private crd: ChangeDetectorRef) {}
  ngOnInit(): void {
    this.ourServices.gitAllServices().subscribe({
        next:(data) => {
        this.Services = data;
       this.crd.detectChanges();
        console.log(this.Services);
      }
      , error: (err) => {
        console.error('Error fetching services:', err);
      }
    });

  }


}
