import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-confirm-mail',
  imports: [],
  templateUrl: './confirm-mail.html',
  styleUrl: './confirm-mail.css'
})
export class ConfirmMail {
  @Input() emailAddress: string = '';
  @Input() message: string = 'We have sent a confirmation email to your email address. Please check your inbox.';

}
