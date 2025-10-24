export interface IEditServiceProvider {
     id: number;
  firstName: string;
  lastName: string;
  address: string;
  gender: number; // enum في الباك، نوعه int
  birthDate: string; // DateOnly → بصيغة "yyyy-MM-dd"
  specialization: number; // enum int
  userImage: File | null; // IFormFile → File من input type="file"
  clinicLocation: string | null;
  description: string;
  examinationType: number; // enum int
  pricePerHour: number;
  numberOfExp: number;
  experience: string | null;
  certificates: IAddCertificate[];
}
export interface IAddCertificate {
  serviceProviderId: number;
  certificateName: string;
  description: string | null;
  issueDate: string; // DateTime → string بصيغة "yyyy-MM-dd"
  image: File; // IFormFile
}