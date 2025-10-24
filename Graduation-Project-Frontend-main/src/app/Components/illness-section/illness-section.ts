import { Subscription } from 'rxjs';
import { IllnessService } from './../../Services/illness-service';
import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { IllnessCard } from "../illness-card/illness-card";
import { EmptyComponent } from "../empty-component/empty-component";
import { LoadingComponent } from "../loading-component/loading-component";

@Component({
  selector: 'app-illness-section',
  imports: [IllnessCard, EmptyComponent, LoadingComponent  ],
  templateUrl: './illness-section.html',
  styleUrl: './illness-section.css'
})
export class IllnessSection implements OnInit , OnDestroy {
  subscription! : Subscription
  constructor(private IllnessService : IllnessService , private changeDetectorRef : ChangeDetectorRef) { }
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
  ngOnInit(): void {
     this.subscription  = this.IllnessService.getIllnesses().subscribe( {
      next : data => {
        this.illnesses = data ;
        this.isLoading = false;
        this.changeDetectorRef.detectChanges();

      },
      error : err => console.log(err) 
    });
  }
  illnesses! : IDisplayIllnessModel[] ;
  isLoading : boolean = true

}
