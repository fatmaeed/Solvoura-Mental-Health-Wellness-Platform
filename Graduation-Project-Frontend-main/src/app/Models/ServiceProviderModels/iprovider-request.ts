export interface IProviderRequest {

    sessionId:number,
    isApproved:boolean,
    clientRequetStatus:RequestStatus|any,
    newStartDateTime:string|null,
    notes:string,

}

export enum RequestStatus{
     Posponed,
     Canceled
}
