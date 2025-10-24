import { Component } from '@angular/core';
import { ServiceProviderCard } from "../../../ServiceProvider/service-provider-card/service-provider-card";
import { ServiceProviderList } from "../../../ServiceProvider/service-provider-list/service-provider-list";

@Component({
  selector: 'app-providers-for-client',
  imports: [ ServiceProviderList],
  templateUrl: './providers-for-client.html',
  styleUrl: './providers-for-client.css'
})
export class ProvidersForClient {

}
