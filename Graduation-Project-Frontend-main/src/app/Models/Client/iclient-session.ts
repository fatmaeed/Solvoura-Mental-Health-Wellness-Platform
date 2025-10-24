

export interface IClientSession {
  id: number;
  doctorName: string;
  doctorspecialization: string;
  userImagePath :string;
  duration :string; 
  startDateTime: string;  // use string because it comes as ISO string from API
  endDateTime: string;
  type: 'Online' | 'Offline';
  status: 'NotStarted' | 'InProgress' | 'Done' | 'Posponed' | 'Canceled'|'Started'|'Finished'
  |'AcceptPosponed '|'AcceptCancelation';
  canCancelOrPostpone: boolean;
  price: number;
}
