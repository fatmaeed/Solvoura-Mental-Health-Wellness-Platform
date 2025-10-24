import { IOurService } from './../../../Models/OurService/iour-service';
import { ChangeDetectorRef, Component } from '@angular/core';
import { OurServiceServices } from '../../../Services/OurServiceServices/ourServiceServices';
import { AdminSideBar } from "../../../Layouts/admin-side-bar/admin-side-bar";
import { Router } from '@angular/router';

@Component({
  selector: 'app-all-our-services',
  imports: [AdminSideBar],
  templateUrl: './all-our-services.html',
  styleUrl: './all-our-services.css'
})
export class AllOurServices {


  constructor(private ourServiceService:OurServiceServices,private cdr:ChangeDetectorRef,private router:Router){}
  ourServices!:IOurService[];

  ngOnInit(){

    this.ourServiceService.getAllOurServices().subscribe((data)=>{
      this.ourServices = data;
      console.log('Services from API:', this.ourServices);

      this.cdr.detectChanges();
    });
    console.log(this.ourServices);

  }
  deleteOurService(id:number){
    this.ourServiceService.deleteOurService(id).subscribe(()=>{
      this.ourServices = this.ourServices.filter(s=>s.id !== id);
      this.cdr.detectChanges();
      window.location.reload();
    });

  }
  goToAddService() {
    this.router.navigate(['/admin-dashboard/add-new-service']);
    }
  goToDetails(id:number){
    this.router.navigate(['/admin-dashboard/details-update-our-services',id],{
      state: { isEditMode: false }

    });
  }
  goToUpdate(id:number){
    this.router.navigate(['/admin-dashboard/details-update-our-services',id],{
      state: { isEditMode: true }
    });


  }


}
