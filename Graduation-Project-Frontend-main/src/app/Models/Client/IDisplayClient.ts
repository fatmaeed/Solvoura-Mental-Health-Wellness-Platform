export interface IDisplayClient {
  id:number;
  userImagePath?:string|null;
  firstName?:string;
  lastName?:string;
  isAnon?: boolean;
  isVerified?: boolean;
  birthDate?: string;
  address?: string;
  gender?: string | number; // Accept number if mapped from <select>
  phoneNumber?: string;
  email?: string;
  alternativePhoneNumber?: string;
  historyIllness?: string;
}
