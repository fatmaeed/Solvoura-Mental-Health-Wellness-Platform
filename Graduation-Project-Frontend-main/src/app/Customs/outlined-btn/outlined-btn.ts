import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-outlined-btn',
  imports: [],
  templateUrl: './outlined-btn.html',
  styleUrl: './outlined-btn.css'
})
export class OutlinedBtn {
  @Input({ required: true }) text = 'Button';
  @Input() href: string | null = null;
  @Input() action?: () => void;

}
