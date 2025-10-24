export interface Post {
  id: number;
  clientId?: number|null;
  serviceProviderId?: number|null;
  body: string;
  imagePath?: string;
  comments?: Comment[]; 
  date:string , 
  likes:number ,
  isLikedByCurrentUser: boolean ,
  userLikeId:number |null
}
