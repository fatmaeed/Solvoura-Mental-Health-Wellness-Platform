import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-custom-search',
  imports: [FormsModule],
  templateUrl: './custom-search.html',
  styleUrl: './custom-search.css'
})
export class CustomSearch {
 @Input() searchQuery: string = '';

  onSearch() {
    console.log('Searching for:', this.searchQuery);
    // Add logic to emit value or call a service
  }
}
