export interface ISessionForReserv {
  id:number;
  startDateTime: string;     
  duration: string;          
  status: 'NotStarted' | 'InProgress' | 'Completed'; 
  type: 'Offline' | 'Online'|'Both';                         
  sessionPrice: number;      
}
