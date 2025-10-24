import { Component, OnInit } from '@angular/core';
import { CustomAddPostContainer } from "../../Customs/custom-add-post-container/custom-add-post-container";
import { CustomPostCard } from "../../Customs/custom-post-card/custom-post-card";
import { Router } from '@angular/router';
import { TokenHandlerService } from '../../Services/Auth/token-handler-service';


@Component({
  selector: 'app-community',
  imports: [CustomAddPostContainer, CustomPostCard],
  templateUrl: './community.html',
  styleUrl: './community.css'
})
export class Community implements OnInit {
  role: string | null = null;

  constructor(
    private router: Router,
    private tokenHandler: TokenHandlerService
  ) {}

  ngOnInit() {
    this.role = this.tokenHandler.Role;
  }

  navigateToHome() {
    if (this.role === 'CLIENT') {
      this.router.navigate(['/home']);
    } else if (this.role === 'SERVICEPROVIDER') {
      this.router.navigate(['/service-provider-dashboard']);
    }
    else if (this.role === 'ADMIN') {
      this.router.navigate(['/admin-dashboard']);
    }
    else{
      this.router.navigate(['/home']);
    }
  }
}
