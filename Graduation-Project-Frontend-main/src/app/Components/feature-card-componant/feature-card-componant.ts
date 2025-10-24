import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IOurService } from '../../Models/OurService/iour-service';

export interface Feature {
  icon: string;        // Bootstrap‑icons class, e.g. "bi-camera-video"
  title: string;       // e.g. "Secure Video Sessions"
  subtitle: string;    // e.g. "HIPAA‑compliant video calls…"
}

@Component({
  selector: 'feature-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './feature-card-componant.html',
  styleUrls: ['./feature-card-componant.css'],
})
export class FeatureCardComponent {
  @Input({ required: true }) feature!: IOurService;
}
