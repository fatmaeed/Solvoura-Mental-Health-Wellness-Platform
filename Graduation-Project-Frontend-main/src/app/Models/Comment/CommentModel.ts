export interface CommentModel {
  id: number;
  postId: number;
  clientId?: number|null;
  serviceProviderId?: number|null;
  body: string;
}
