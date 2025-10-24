import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-custom-btn',
  imports: [],
  templateUrl: './custom-btn.html',
  styleUrl: './custom-btn.css'
})
export class CustomBtn {
  @Input() text: string = 'Get Started';
  @Input() href: string | null = null;
  @Input() action?: () => void;
}
