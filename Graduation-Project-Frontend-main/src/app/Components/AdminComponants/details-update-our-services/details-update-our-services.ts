import { ChangeDetectorRef, Component } from '@angular/core';
import { AdminSideBar } from "../../../Layouts/admin-side-bar/admin-side-bar";
import { IOurService } from '../../../Models/OurService/iour-service';
import { OurServiceServices } from '../../../Services/OurServiceServices/ourServiceServices';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-details-update-our-services',
  imports: [AdminSideBar,FormsModule,CommonModule],
  templateUrl: './details-update-our-services.html',
  styleUrl: './details-update-our-services.css'
})
export class DetailsUpdateOurServices {
  isEditMode = false;

baseUrl:string = environment.apiBaseUrl;
  ourService!:IOurService ;
  icon!: string;


  iconOptions: string[] = [
    'bi-alarm', 'bi-app', 'bi-archive', 'bi-arrow-down', 'bi-arrow-left', 'bi-arrow-right', 'bi-arrow-up',
    'bi-bag', 'bi-bell', 'bi-bookmark', 'bi-box', 'bi-briefcase', 'bi-calendar', 'bi-camera', 'bi-chat',
    'bi-check', 'bi-check-circle', 'bi-chevron-down', 'bi-chevron-left', 'bi-chevron-right', 'bi-chevron-up',
    'bi-circle', 'bi-clock', 'bi-cloud', 'bi-code', 'bi-collection', 'bi-credit-card', 'bi-database', 'bi-diagram-3',
    'bi-display', 'bi-download', 'bi-envelope', 'bi-exclamation', 'bi-eye', 'bi-file', 'bi-filter', 'bi-flag',
    'bi-folder', 'bi-gear', 'bi-gift', 'bi-grid', 'bi-heart', 'bi-house', 'bi-image', 'bi-info-circle',
    'bi-key', 'bi-lightning', 'bi-link', 'bi-list', 'bi-lock', 'bi-map', 'bi-megaphone', 'bi-mic',
    'bi-moon', 'bi-music-note', 'bi-palette', 'bi-paperclip', 'bi-pencil', 'bi-people', 'bi-person', 'bi-phone',
    'bi-pin', 'bi-play', 'bi-plus', 'bi-printer', 'bi-question-circle', 'bi-reply', 'bi-search', 'bi-send',
    'bi-share', 'bi-shield', 'bi-star', 'bi-sun', 'bi-tablet', 'bi-tag', 'bi-trash', 'bi-upload',
    'bi-x', 'bi-x-circle',

    'bi-facebook', 'bi-twitter', 'bi-instagram', 'bi-linkedin', 'bi-youtube', 'bi-reddit', 'bi-discord', 'bi-github',
    'bi-google', 'bi-microsoft', 'bi-slack', 'bi-spotify', 'bi-twitch', 'bi-whatsapp', 'bi-telegram', 'bi-tiktok',

    'bi-credit-card-2-back', 'bi-credit-card-2-front', 'bi-paypal', 'bi-coin', 'bi-cash', 'bi-wallet',

    'bi-cpu', 'bi-display', 'bi-hdd', 'bi-motherboard', 'bi-phone', 'bi-router', 'bi-server', 'bi-usb',

    'bi-bar-chart', 'bi-bank', 'bi-basket', 'bi-basket2', 'bi-basket3', 'bi-cart', 'bi-cart2', 'bi-cart3',
    'bi-cart4', 'bi-cash-stack', 'bi-currency-dollar', 'bi-currency-euro', 'bi-currency-pound', 'bi-currency-yen',
    'bi-graph-up', 'bi-pie-chart', 'bi-receipt', 'bi-shop', 'bi-truck'
  ];

  selectIcon(selected: string) {
    this.icon = selected;
    this.ourService.icon = this.icon;
  }


  constructor(private ourServiceService:OurServiceServices,private cdr:ChangeDetectorRef,private route:ActivatedRoute,private router:Router){}
  ngOnInit(){
    const id = Number(this.route.snapshot.paramMap.get('id'));
    const navState = window.history.state;

    this.isEditMode = navState?.isEditMode ?? false;

    this.ourServiceService.getOurServiceById(id).subscribe((data) => {
      this.ourService = data;
      console.log('Service from API:', this.ourService);
      this.cdr.detectChanges();
    });
  }
  onSave() {
    this.ourServiceService.updateOurService(this.ourService.id, this.ourService)
      .subscribe(() => {
        alert('Service updated successfully');
        this.isEditMode = false;
        this.cdr.detectChanges();
                              this.router.navigate(['/admin-dashboard/all-our-services']);


      });
  }
}

