import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-custom-alert',
  imports: [],
  templateUrl: './custom-alert.html',
  styleUrl: './custom-alert.css'
})
export class CustomAlert {
  @Input() message: string = '';
  @Input() visible: boolean = false;
  @Output() closed = new EventEmitter<void>();

  close() {
    this.closed.emit();
  }
}
