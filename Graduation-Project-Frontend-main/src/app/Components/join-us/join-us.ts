import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-join-us',
  templateUrl: './join-us.html',
  styleUrl: './join-us.css'
})
export class JoinUs {
  constructor(private router: Router) {}
  goToSignUp() {
    this.router.navigate(['/signup']);
  }
}
