import { RequestStatus } from "./iprovider-request";

export interface IdisplaySessionWithStatus {

id:number,
startDateTime:string,
endDateTime:string,
sessionPrice:number,
type:string,
status:RequestStatus,
clientName:string
}
