export enum SessionActionType {
  Cancel = 0,
  Postpone = 1
}

export interface IClientRequest {
  clientId:number
  sessionId: number ;
  reason: string;
  actionType: SessionActionType;
}

