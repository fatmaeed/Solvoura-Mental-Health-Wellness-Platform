import { Component } from '@angular/core';

@Component({
  selector: 'app-feed-back-section',
  imports: [],
  templateUrl: './feed-back-section.html',
  styleUrl: './feed-back-section.css'
})
export class FeedBackSection {
testimonials = [
    {
      name: 'Sarah J.',
      image: 'https://randomuser.me/api/portraits/women/43.jpg',
      stars: 5,
      role: 'Therapy Client',
      text: 'MindConnect changed my life. My therapist understood me perfectly and the platform made scheduling so easy. After 3 months, my anxiety is manageable for the first time in years.',
      joined: 'Joined 5 months ago'
    },
    {
      name: 'Michael T.',
      image: 'https://randomuser.me/api/portraits/men/32.jpg',
      stars: 4.5,
      role: 'Psychiatry Patient',
      text: 'The psychiatrist I found here was incredibly knowledgeable. The video sessions fit perfectly with my busy schedule. Medication management has never been this convenient.',
      joined: 'Joined 8 months ago'
    },
    {
      name: 'Priya K.',
      image: 'https://randomuser.me/api/portraits/women/65.jpg',
      stars: 5,
      role: 'Life Coaching Client',
      text: 'My coach helped me rebuild my confidence after burnout. The exercises and weekly sessions gave me structure I needed. I\'ve since been promoted at work!',
      joined: 'Joined 3 months ago'
    }
  ];

  getFilledStars(rating: number): number[] {
    const fullStars = Math.floor(rating);
    return Array(fullStars).fill(0);
  }
}
