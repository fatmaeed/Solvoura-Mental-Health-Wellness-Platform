import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-illness-card',
  imports: [CommonModule],
  templateUrl: './illness-card.html',
  styleUrl: './illness-card.css'
})
export class IllnessCard {
@Input({required: true}) illness! : IDisplayIllnessModel;

showImage = true;

ngOnInit() {
  if (!this.illness.image || this.illness.image.trim() === '') {
    this.showImage = false;
  }
}

onImageError() {
  this.showImage = false;
}

}
