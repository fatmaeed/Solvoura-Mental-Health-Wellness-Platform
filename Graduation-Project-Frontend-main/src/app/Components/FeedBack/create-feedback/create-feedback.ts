import { ActivatedRoute, Router } from '@angular/router';
import { TokenHandlerService } from './../../../Services/token-handler-service';
import { ChangeDetectorRef, Component, Input, OnInit, Output } from '@angular/core';
import { FeedBackService } from '../../../Services/feed-back-service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ICreateFeedBack } from '../../../Models/FeedBack/icreate-feed-back';

@Component({
  selector: 'app-create-feedback',
  imports: [CommonModule , FormsModule, ],
  templateUrl: './create-feedback.html',
  styleUrl: './create-feedback.css'
})
export class CreateFeedback  implements OnInit {

  constructor(private feedBackService:FeedBackService , private cdr:ChangeDetectorRef , 
    private tokenHandlerService:TokenHandlerService,
    private activatedRoute:ActivatedRoute,
    private router:Router
  
  ){}
  ngOnInit(): void {
    this.evaluatorId = this.tokenHandlerService.UserId;
    this.sessionId = Number(this.activatedRoute.snapshot.paramMap.get('sessionId'));
    console.log(this.sessionId);
  }

 evaluatorId!: number | null ;
  sessionId!: number ;

  stars = [1, 2, 3, 4, 5];
  rate = 0;
  hoverRating = 0;
  comment = '';
  isSubmitting = false;

  setRating(rating: number): void {
    this.rate = rating;
  }

  setHoverRating(rating: number): void {
    this.hoverRating = rating;
  }

  clearHoverRating(): void {
    this.hoverRating = 0;
  }

  submitFeedback(): void {
    if (this.rate === 0) return;

    this.isSubmitting = true;
    const feedback = {
      evaluatorId: this.evaluatorId,
      sessionId: this.sessionId,
      rate: this.rate,
      comment: this.comment
    } as ICreateFeedBack ;
        this.feedBackService.createFeedBack(feedback).subscribe({
        next: (response) => {
          this.isSubmitting = false ;
          this.rate = 0;
          this.comment = ''
          this.cdr.detectChanges() ;
          if(this.tokenHandlerService.Role === 'SERVICEPROVIDER') {
            this.router.navigate(['/service-provider-dashboard']);
          }else if(this.tokenHandlerService.Role === 'CLIENT') {
            this.router.navigate(['/home']);
          }
         },
        error: (error) => { console.error('Error creating FeedBack:', error); }
      });
      

  }


}
