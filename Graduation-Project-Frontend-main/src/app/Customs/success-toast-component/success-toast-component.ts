import { Component, Inject, Input } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-success-toast-component',
  imports: [],
  templateUrl: './success-toast-component.html',
  styleUrl: './success-toast-component.css'
})
export class SuccessToastComponent {
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: { message: string }
  ) {}

  closeDialog(): void {
  }
}




