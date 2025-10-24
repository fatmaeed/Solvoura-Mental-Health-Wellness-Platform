export interface IRegisterClient {
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  password: string;
  confirmPassword: string;
  userName: string;
  birthDate: string;
  address: string;
  illnessesHistory: string;
  gender: number;
  neededSpecilization: number;
  UserImage?:string|null;
}
