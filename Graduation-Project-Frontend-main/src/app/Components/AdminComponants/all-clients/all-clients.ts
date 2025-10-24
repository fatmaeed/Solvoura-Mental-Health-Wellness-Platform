import { ChangeDetectorRef, Component } from '@angular/core';
import { ClientService } from '../../../Services/ClientService/clientService';
import { IDisplayClient } from '../../../Models/Client/IDisplayClient';
import { AdminSideBar } from "../../../Layouts/admin-side-bar/admin-side-bar";
import { Router } from '@angular/router';

@Component({
  selector: 'app-all-clients',
  imports: [AdminSideBar],
  templateUrl: './all-clients.html',
  styleUrl: './all-clients.css'
})
export class AllClients {

  constructor(private clientService: ClientService,private cdr:ChangeDetectorRef,private router:Router){}
  clients:IDisplayClient[] = [];
  ngOnInit(){
    this.clientService.getAllClients().subscribe((data)=>{
      this.clients = data;
      this.cdr.detectChanges();
      console.log(this.clients);
    });
  }
  deleteClient(id: number) {
    this.clientService.deleteClient(id).subscribe(() => {
      this.clients = this.clients.filter(c => c.id !== id);
      this.cdr.detectChanges();
      window.location.reload();
    });
    }
    goToUpdate(id: number) {
      this.router.navigate(['/admin-dashboard/details-update-componantfor-clients', id], {
        state: { isEditMode: true }
      });
    }
    goToDetails(id: number) {
      this.router.navigate(['/admin-dashboard/details-update-componantfor-clients', id], {
        state: { isEditMode: false }
      });
    }
}
