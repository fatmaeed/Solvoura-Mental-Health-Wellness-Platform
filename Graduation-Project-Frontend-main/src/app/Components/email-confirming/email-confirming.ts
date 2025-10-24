import { AccountService } from './../../Services/Auth/account-service';
import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { LoadingComp } from "../loading/loading";

@Component({
  selector: 'app-email-confirming',
  imports: [
    CommonModule,
    RouterModule,
    MatButtonModule,
    LoadingComp
],
  templateUrl: './email-confirming.html',
  styleUrl: './email-confirming.css'
})
export class EmailConfirming {
  constructor(private accountService: AccountService , private activatedRoute: ActivatedRoute , private router: Router) { }

  ngOnInit(): void {

    this.accountService.confirmEmail(this.activatedRoute.snapshot.queryParamMap.get('token')! , this.activatedRoute.snapshot.queryParamMap.get('email')!).subscribe({
      next: (data) => {
        localStorage.setItem('token', data.token);
        this.router.navigate(['/home']);
      },
      error: (err) => {
        console.error('Error fetching service provider:', err);
      }
    });
  }
}
