export interface CreateComment {
  postId: number;
  clientId?: number|null;
  serviceProviderId?: number|null;
  body: string;
}
