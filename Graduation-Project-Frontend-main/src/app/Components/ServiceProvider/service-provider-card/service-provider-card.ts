import {
  Component,
  Input,
  OnInit,
} from '@angular/core';
import { IDisplayServiceProvider } from '../../../Models/ServiceProviderModels/idisplay-service-provider';
import { CommonModule } from '@angular/common';
import { AccountService } from '../../../Services/Auth/account-service';
import { Router } from '@angular/router';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-service-provider-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './service-provider-card.html',
  styleUrl: './service-provider-card.css',
})
export class ServiceProviderCard implements OnInit {
  @Input() provider!: IDisplayServiceProvider;
  showImage: boolean = true;
  apiUrl:string = `${environment.apiBaseUrl}`

  constructor (private router: Router, private authService: AccountService) {}
  ngOnInit() {
    if (!this.provider.userImagePath?.trim()) {
      this.showImage = false;
    }
  }

  onImageError(): void {
    this.showImage = false;
  }

  get initials(): string {
    return (
      this.provider.firstName.charAt(0) + this.provider.lastName.charAt(0)
    ).toUpperCase();
  }
  handleBooking() {
    if (this.authService.isLoggedIn()) {
      this.router.navigate(['/reservation', this.provider.id]);
    } else {
      this.router.navigate(['/login']);
    }}
  get avatarStyle(): { [key: string]: string } {
    const hue = this.provider.userImagePath ?? 200;
    return {
      background: `linear-gradient(135deg, hsl(${hue}, 60%, 55%), hsl(${hue}, 70%, 85%))`,
      color: '#fff',
    };
  }

  get fullStars(): number[] {
    return Array(Math.floor(+this.provider.numberOfExp)).fill(0);
  }
}
