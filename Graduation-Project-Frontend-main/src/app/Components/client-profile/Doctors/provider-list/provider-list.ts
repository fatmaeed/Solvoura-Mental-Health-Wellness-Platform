import { Component } from '@angular/core';
import { ServiceProviderList } from "../../../ServiceProvider/service-provider-list/service-provider-list";

@Component({
  selector: 'app-provider-list',
  imports: [ServiceProviderList],
  templateUrl: './provider-list.html',
  styleUrl: './provider-list.css'
})
export class ProviderList {

}
