import { ICreateCertificateDTO } from "../certificate/ICreateCertificateDTO";
export interface IRegisterServiceProvider {
  FirstName: string;
  LastName: string;
  UserName: string;
  Email: string;
  PhoneNumber: string;
  Password: string;
  ConfirmPassword: string;
  Gender: number;
  BirthDate: string;
  Address: string;
  NationalNumber: string;
  Specialization: number;
  UserImage: File;
  NationalImage: File;
  UserAndNationalImage: File;
  ClinicLocation?: string;
  Description: string;
  ExaminationType: number;
  ExperienceInYears: number;
  ExperienceDescription?: string;
  PricePerHour: number;
  Certificates: ICreateCertificateDTO[];
  
}
