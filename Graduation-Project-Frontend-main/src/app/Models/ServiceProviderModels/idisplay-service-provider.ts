import { ICreateCertificateDTO } from "../certificate/ICreateCertificateDTO"

export interface IDisplayServiceProvider {
    id:number,
    specialization: string,
    userImagePath: string,
    description: string
    numberOfExp: number,
    experience: string ,
    firstName: string,
    lastName: string,
    address:string,
    clinicLocation: string,
    pricePerHour: number,
     gender: number;
   birthDate: Date | string;
   examinationType: number;
   certificatesAdd :AddCertificateDTO[] ,
   isApproved: boolean,
    userAndNationalImagePath: string,
    nationalImagePath: string,
    certificates:ICreateCertificateDTO[],
}
export interface AddCertificateDTO {
  serviceProviderId: number;
  image: File;
  certificateName: string;
  description?: string;
  issueDate: Date;
}
    

