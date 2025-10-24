import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-empty-component',
  imports: [],
  templateUrl: './empty-component.html',
  styleUrl: './empty-component.css'
})
export class EmptyComponent {
  @Input() message: string = 'No data available';

}
