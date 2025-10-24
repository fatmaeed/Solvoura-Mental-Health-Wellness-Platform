export interface IResrvationRequest {
     serviceProviderId: number;
  clientId: number |any;
   paymentId: number;
  status: 'Offline' | 'Online'|string;
  sessionsNumber: number;
  sessionIds: number[];
}
