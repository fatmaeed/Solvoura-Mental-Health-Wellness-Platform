import { ClientRequest } from './Components/ServiceProvider/client-request/client-request';
import { Routes } from '@angular/router';
import { ServiceProviderList } from './Components/ServiceProvider/service-provider-list/service-provider-list';
import { Home } from './Views/home/home';
import { NotFound } from './Views/not-found/not-found';
import { Login } from './Components/Auth/login/login';
import { SignUp } from './Components/Auth/sign-up/sign-up';
import { ForgetPassword } from './Components/Auth/forget-password/forget-password';
import { ResetPassword } from './Components/Auth/reset-password/reset-password';
import { ChangePassword } from './Components/change-password/change-password';
import { UserNameGuard } from './Common/change-password-guard';
import { authGuard } from './Common/auth-guard';
import { Reservation } from './Components/Reservation/reservation/reservation';
import { CreateSessions } from './Components/ServiceProvider/create-sessions/create-sessions';
import { DisplaySessionsToSp } from './Components/ServiceProvider/display-sessions-to-sp/display-sessions-to-sp';
import { ConfirmMail } from './Components/confirm-mail/confirm-mail';
import { AllServiceProviders } from './Components/all-service-providers/all-service-providers';
import { Community } from './Views/community/community';
import { ClientProfile } from './Components/client-profile/client-profile';
import { ClientSession } from './Components/client-profile/ClientSession/client-session/client-session';
import { ProvidersForClient } from './Components/client-profile/ProvidersForClient/providers-for-client/providers-for-client';
import { EmailConfirming } from './Components/email-confirming/email-confirming';
import { MeetingSession } from './Components/meeting-session/meeting-session';
import { ProviderList } from './Components/client-profile/Doctors/provider-list/provider-list';
import { EditServiceProvider } from './Components/ServiceProvider/edit-service-provider/edit-service-provider';
import { PendingServiceProviders } from './Components/AdminComponants/pending-service-providers/pending-service-providers';
import { ServiceProviderDetails } from './Components/AdminComponants/service-provider-details/service-provider-details';
import { AllServiceProvidersForAdmin } from './Components/AdminComponants/all-service-providers-for-admin/all-service-providers';
import { DetailsUpdateComponant } from './Components/AdminComponants/details-update-componant/details-update-componant';
import { AllClients } from './Components/AdminComponants/all-clients/all-clients';
import { DetailsUpdateComponantforClients } from './Components/AdminComponants/details-update-componantfor-clients/details-update-componantfor-clients';
import { AllOurServices } from './Components/AdminComponants/all-our-services/all-our-services';
import { DetailsUpdateOurServices } from './Components/AdminComponants/details-update-our-services/details-update-our-services';
import { AddNewService } from './Components/AdminComponants/add-new-service/add-new-service';
import { CreateFeedback } from './Components/FeedBack/create-feedback/create-feedback';
import { ClientInfo } from './Components/client-profile/client-info/client-info';
import { SideBarServiceProvider } from './Layouts/side-bar-service-provider/side-bar-service-provider';
import { AdminSideBar } from './Layouts/admin-side-bar/admin-side-bar';
import { IncomingSession } from './Components/ServiceProvider/incoming-session/incoming-session';
import { RoleRedirectGuard } from './Common/role-guard';
import { adminGuard } from './Common/admin-guard';
export const routes: Routes = [
  { path: 'home', component: Home, canActivate: [RoleRedirectGuard] },
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  // { path: 'service-provider-list', component: ServiceProviderList , canActivate: [authGuard, UserNameGuard]},
  { path: 'login', component: Login },
  { path: 'signup', component: SignUp },
  { path: ':username/profile/change-password', component: ChangePassword  , canActivate:[authGuard,UserNameGuard]},
  { path: 'forgot-password', component: ForgetPassword },
  { path: 'reset-password', component: ResetPassword },
  { path: 'confirm-email', component: ConfirmMail },
  { path: 'confirming', component: EmailConfirming },
  { path: 'reservation/:id', component: Reservation  },
  { path: 'community', component: Community  },
  { path: 'all-service-providers', component: AllServiceProviders  },
  {
    path: 'client-profile', component: ClientProfile, children: [
      { path: '', redirectTo: 'client-info', pathMatch: 'full' },

      { path: 'client-session', component: ClientSession },
      { path: 'providers-for-client', component: ProvidersForClient },
      { path: 'provider-list', component: ProviderList },
      {path:'client-info',component:ClientInfo},
    ]
  },
  {
    path: 'service-provider-dashboard',
    component: SideBarServiceProvider,
    children: [
      { path: '', redirectTo: 'display-sessions-to-sp', pathMatch: 'full' },

      { path: 'display-sessions-to-sp', component: DisplaySessionsToSp },
      { path: 'add-sessions', component: CreateSessions },
      { path: 'client-request', component: ClientRequest },
      { path: 'edit-service-provider', component: EditServiceProvider },
      {path:'incoming-session',component:IncomingSession},

    ]
  },
  {
    path: 'admin-dashboard',
    component: AdminSideBar,
    children: [
      { path: '', redirectTo: 'pending-service-providers', pathMatch: 'full' },

      {path: 'all-clients', component: AllClients , children:[

      ]} ,
      {path: 'details-update-componantfor-clients/:id', component: DetailsUpdateComponantforClients} ,
      {path: 'details-update-componant/:id', component: DetailsUpdateComponant} ,
      {path: 'all-service-providers-for-admin', component: AllServiceProvidersForAdmin} ,
      {path: 'service-provider-details/:id', component: ServiceProviderDetails} ,
      {path: 'pending-service-providers', component: PendingServiceProviders} ,
      {path: 'all-our-services', component: AllOurServices } ,
      {path: 'details-update-our-services/:id', component: DetailsUpdateOurServices} ,
      {path: 'add-new-service', component: AddNewService }

    ]
  },

  {path : 'confirming' , component : EmailConfirming} ,
  {path :'meeting/:id' , component : MeetingSession} ,
  { path: 'reservation/:id', component : Reservation } ,
  {path: 'community', component: Community} ,
  {path: 'all-service-providers', component: AllServiceProviders} ,
  {path: 'add-sessions', component: CreateSessions} ,
  {path: 'client-profile', component: ClientProfile,children:[
 {path:'client-session',component : ClientSession},
  {path:'providers-for-client',component : ProvidersForClient},
  ] } ,
  {path: 'all-clients', component: AllClients} ,
  {path: 'details-update-componantfor-clients/:id', component: DetailsUpdateComponantforClients} ,
  {path: 'details-update-componant/:id', component: DetailsUpdateComponant} ,
  {path: 'service-provider-details/:id', component: ServiceProviderDetails} ,

  {path: 'create-feedback/:sessionId' , component:CreateFeedback } ,
  { path: '**',component: NotFound,pathMatch: 'full'} ,
];

//navigation of feedback
