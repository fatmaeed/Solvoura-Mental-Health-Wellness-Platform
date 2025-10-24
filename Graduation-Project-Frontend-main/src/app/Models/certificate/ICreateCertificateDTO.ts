export interface ICreateCertificateDTO {
  image: File;

  certificateName: string;
  description?: string;

  issueDate: string; 
}
