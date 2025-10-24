import { CommonModule } from '@angular/common';
import { Component, Input  } from '@angular/core';
import { Router } from '@angular/router';
import { INotificationModel } from '../../Models/Notification/INotificationModel';

@Component({
  selector: 'app-notification',
  imports: [CommonModule],
  templateUrl: './notification.html',
  styleUrl: './notification.css'
})
export class Notification  {
 
@Input({ required: true}) notification!: INotificationModel;
 @Input({ required: true}) isVisible = false;


constructor(private router: Router) {
 
}



  hide() {
    this.isVisible = false;
  }
  handleClick() {
    if (this.notification?.routing) {
      this.router.navigate([this.notification.routing]);
      this.hide();
    }
  }
  }


