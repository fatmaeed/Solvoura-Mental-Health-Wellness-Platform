import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-not-found',
  imports: [],
  templateUrl: './not-found.html',
  styleUrl: './not-found.css'
})
export class NotFound {
  @Input() requestedUrl: string | null = null;

  constructor(private router: Router) {}

  goHome() {
    this.router.navigateByUrl('/');
  }


}
