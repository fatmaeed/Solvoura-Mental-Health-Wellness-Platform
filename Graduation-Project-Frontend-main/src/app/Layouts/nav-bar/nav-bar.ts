import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterModule } from '@angular/router';
import { JwtHelperService } from '../../Services/Auth/jwt-helper-service';
import { AccountService } from '../../Services/Auth/account-service';
import { NotificationService } from '../../Services/notification-service';
import { TokenHandlerService } from '../../Services/token-handler-service';
import { INotification, INotificationUpdate } from '../../Models/Notification/INotificationModel';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-nav-bar',
  imports: [CommonModule, RouterModule,RouterLink,FormsModule],
  templateUrl: './nav-bar.html',
  styleUrl: './nav-bar.css'
})
export class NavBar implements OnInit {
isOpen = false;
notifications!:INotification[];
notification!:INotification;
goToNotifications() {
throw new Error('Method not implemented.');
}
  isMenuOpen = false;
  userName!: string|null ;
  userId!:number|null;
 unreadCount!:number;
  constructor(private router: Router ,public accountService: AccountService, private jwtHelper: JwtHelperService,private cdr:ChangeDetectorRef,private service:NotificationService,private tokenhandler :TokenHandlerService) {}

  ngOnInit(): void {
    const token = this.jwtHelper.getDecodedToken();
    this.cdr.detectChanges();
    this.userName = this.tokenhandler.UserName;
    this.userId = this.tokenhandler.UserId;
    this.getUnreadnoti();
     
  }

  toggleMenu() {
    this.isMenuOpen = !this.isMenuOpen;
  }
  goToLogin() {
    this.router.navigate(['/login']);
  }
  goToSignUp() {
    this.router.navigate(['/signup']);
  }
  logout(): void {
    this.accountService.logout();
  }

  // goToProfile(): void {
  //   this.router.navigate(['profile']);
  // }

  goToSection(sectionId: string) {
    const element = document.getElementById(sectionId);

    if (element) {

      element.scrollIntoView({ behavior: 'smooth', block: 'start' });

    }
  }
  toggleDropdown(): void {
    this.isOpen = !this.isOpen;  
    this.loadNotification()
  }
  loadNotification(){
    if(!this.userId) return
     this.service.getNotificationForUser(this.userId).subscribe({
      next:(data)=>{
        console.log("Data" ,data);
       this.notifications = data;
       this.cdr.detectChanges()
       console.log(this.notifications)
      },
      error:(err)=>{
        console.log("Error fetching Notification",err)
      }
     })
  }

  markAllAsRead(): void {
    for(const noti of this.notifications){
      this.update(noti);
      this.cdr.detectChanges();
    }
  }
   handleNotificationClick(notification: INotification): void {
    if (!notification.readed) {
      notification.readed = true;
      this.unreadCount--;
    }
     if (notification.routing) {
    this.router.navigate([notification.routing]);
  }
  }

  route:any;
  goto(id:number){
    this.service.getById(id).subscribe({
      next:(data)=>{
         this.notification =data;
         this.cdr.detectChanges();
          if (this.notification?.routing) {
        this.router.navigate([this.notification.routing]);
        this.isOpen = false;
        this.update(this.notification);
        this.cdr.detectChanges();
      } else {
        console.warn("Routing is missing in the notification data");
      }
      },
      error:(err)=>{
        console.log(err);
      }
    })

  }

  update(request :INotification)
  {
   request.readed =true;
   this.service.updateNoti(request).subscribe({
    next:()=>{
     console.log("updated")
    },
    error:(err)=>{
      console.log(err)
    }
   })

  }
  ureadCount!:number;
  unreadNoti!:INotification[];
  getUnreadnoti(){
    if(!this.userId)return
    this.service.getUnReadNoti(this.userId).subscribe({
      next:(data)=>{
        this.unreadNoti= data;
        this.cdr.detectChanges();
         this.ureadCount = this.unreadNoti.length;
         console.log("count",this.ureadCount)
       
        
      },
      error:(err)=>{
        console.log(err);
      }
    })
  }



  }


