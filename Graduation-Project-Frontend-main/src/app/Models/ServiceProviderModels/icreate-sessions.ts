
export enum specializationEnumMap  {
   Doctors = 0,
 Specialists = 1,
 SpeechTherapists = 2,
 LifeCoaches = 3,
 Therapists = 4,
};
export enum SessionType {
    Online = 1,
    Offline = 0 , 
    Both = 2
}

export enum RepeatOption {
    Single = 0,
    Daily = 1,
    Weekly = 7
}

export interface ICreateSessions {

    serviceProviderId: number;
    startDateTime: Date;
    durationInMinutes: number;
    type: SessionType;
    repeatedFor: RepeatOption;
    fromDate?: Date;
    toDate?: Date;
}
 export enum SessionStatus {
     NotStarted,
     Started,
     InProgress,
     Posponed,
     Canceled,
     Finished,
     AcceptPosponed,
     AcceptCancelation

 }
export interface IEditSessions{
  id:number,
  serverProvider:number|null,
  startDateTime:Date,
  duration:number,
  status:SessionStatus,
  type:SessionType
}