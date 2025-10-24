 export interface INotificationModel {

 title : string;
 read : boolean;
 message : string;
 type : string;
 routing : string | null;
 senderId : number | null;
 senderName : string | null;
 notificationTime : string;

}
export interface INotification {
   id: number;
  notificationTime: string;
  senderName: string;
  title: string;
  readed: boolean;
  message: string;
  type: string;
  routing: string;
  senderId: number;
}
export interface INotificationUpdate {
 id: number;
  senderName: string;
  title: string;
  readed: boolean;
  message: string;
  type: string;
  routing: string;
  senderId: number;
}