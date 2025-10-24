import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-pendingservice-provider',
  imports: [],
  templateUrl: './pendingservice-provider.html',
  styleUrl: './pendingservice-provider.css'
})
export class PendingserviceProvider {
  @Input() isVerified: boolean | null = null;

}
