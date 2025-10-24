import { Component } from '@angular/core';
import { OurServiceServices } from '../../../Services/OurServiceServices/ourServiceServices';
import { IOurService } from '../../../Models/OurService/iour-service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AdminSideBar } from "../../../Layouts/admin-side-bar/admin-side-bar";
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-new-service',
  standalone: true,
  imports: [FormsModule, CommonModule, AdminSideBar],
  templateUrl: './add-new-service.html',
  styleUrl: './add-new-service.css'
})
export class AddNewService {
  title!: string;
  description!: string;
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
  }

  constructor(private ourServiceService: OurServiceServices, private router: Router) {}


  onSave() {
    const formData = new FormData();
    formData.append('Title', this.title);
    formData.append('Description', this.description);
    formData.append('Icon', this.icon);

    const newService: IOurService = {
      id: 0,
      title: this.title,
      description: this.description,
      icon: this.icon
    };

    this.ourServiceService.addOurService(newService).subscribe({
      next: (data) => {
        console.log('Service added:', data);
                                      this.router.navigate(['/admin-dashboard/all-our-services']);

      },
      error: (err) => console.error('Error:', err)
    });
  }
}
