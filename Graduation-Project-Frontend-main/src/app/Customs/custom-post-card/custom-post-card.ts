import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Post } from '../../Models/Post/PostModel';
import { PostService } from '../../Services/Post&CommentServices/PostService';
import { CommentService } from '../../Services/Post&CommentServices/CommentService';
import { TokenHandlerService } from '../../Services/Auth/token-handler-service';
import { CreateComment } from '../../Models/Comment/CreateComment';
import { CommentModel } from '../../Models/Comment/CommentModel';
import { ClientService } from './../../Services/ClientService/clientService';
import { ServiceProviderService } from './../../Services/service-provider-service';
import { firstValueFrom } from 'rxjs';
import { UserLikeService } from '../../Services/user-like-service';
import { IUserLikes } from '../../Models/iuser-likes';

@Component({
  selector: 'app-custom-post-card',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './custom-post-card.html',
  styleUrls: ['./custom-post-card.css']
})
export class CustomPostCard implements OnInit {
  posts: Post[] = [];
  currentUserId: number | null = null;
  role: string | null = null;
  userNameMap = new Map<string, string>();
  commentCounts: { [postId: number]: number } = {};
  selectedPostId: number | null = null;
  postComments: CommentModel[] = [];
  newCommentBody: string = '';
  dropdownOpenPostId: number | null = null;

  toggleDropdown(postId: number): void {
    this.dropdownOpenPostId = this.dropdownOpenPostId === postId ? null : postId;
  }
    constructor(
    private postService: PostService,
    private commentService: CommentService,
    private tokenHandler: TokenHandlerService,
    private clientService: ClientService,
    private serviceProviderService: ServiceProviderService,
    private cdr: ChangeDetectorRef ,
    private userLikesService:UserLikeService
  ) {
    this.currentUserId = this.tokenHandler.UserId;
    this.role = this.tokenHandler.Role;
  }
  isLoggedIn(): boolean {
    return this.tokenHandler.UserId !== null && this.tokenHandler.Role !== null;
  }

  ngOnInit() {
    this.postService.getAll().subscribe(async posts => {
      this.posts = posts;

      for (const post of posts) {
        if (post.clientId) {
          const client = await firstValueFrom(this.clientService.getClientById(post.clientId));
          this.userNameMap.set(`client-${post.clientId}`, `${client.firstName} ${client.lastName}`);
        } else if (post.serviceProviderId) {
          const sp = await firstValueFrom(this.serviceProviderService.getServiceProviderById(post.serviceProviderId));
          this.userNameMap.set(`sp-${post.serviceProviderId}`, `${sp.firstName} ${sp.lastName}`);
        }

        this.commentService.getByPost(post.id).subscribe(comments => {
          this.commentCounts[post.id] = comments.length;
          if (this.selectedPostId === null) {
            this.selectedPostId = post.id;
            this.postComments = comments;
          }
          this.cdr.detectChanges();
        });
      }
    });
  }


getInitials(name: string): string {
  if (!name) return '';
  const parts = name.split(' ');
  let initials = parts[0].charAt(0).toUpperCase();
  if (parts.length > 1) {
    initials += parts[parts.length - 1].charAt(0).toUpperCase();
  }
  return initials;
}

isAdmin(): boolean {
  return this.role === 'ADMIN';
}

canDeleteComment(comment: any): boolean {
  return this.isAdmin() || comment.clientId === this.currentUserId || comment.serviceProviderId === this.currentUserId;
}
deleteComment(commentId: number): void {
  this.commentService.delete(commentId).subscribe(() => {
    this.postComments = this.postComments.filter(c => c.id !== commentId);
    this.cdr.detectChanges();
  });
}

// isUserPost(post: any): boolean {
//   return post.clientId === this.currentUserId || post.serviceProviderId === this.currentUserId;
// }

  getUserName(clientId?: number | null, serviceProviderId?: number | null): string {
    if (clientId != null) return this.userNameMap.get(`client-${clientId}`) || 'Unknown';
    if (serviceProviderId != null) return this.userNameMap.get(`sp-${serviceProviderId}`) || 'Unknown';
    return 'Unknown';
  }


  isUserPost(post: Post): boolean {
    return (this.currentUserId == post.clientId || this.currentUserId == post.serviceProviderId);
  }

  deletePost(postId: number): void {
    this.postService.delete(postId).subscribe(() => {
      this.posts = this.posts.filter(p => p.id !== postId);
      this.cdr.detectChanges();
    });
  }

  editPost(post: Post | null | undefined): void {
    if (!post || !post.body) return;

    const updatedBody = prompt('Edit your post:', post.body);
    if (updatedBody !== null && updatedBody.trim() !== '') {
      const updatedPost = { ...post, body: updatedBody.trim() };

      this.postService.update(updatedPost).subscribe({
        next: (updated) => {
          if (updated?.body != null) {
            post.body = updated.body;
            this.cdr.detectChanges();
          }
        },
        error: (err) => console.error('Failed to update post:', err)
      });
    }
  }

  toggleComments(postId: number): void {
    if (this.selectedPostId === postId) {

      this.selectedPostId = null;
      this.postComments = [];
    } else {
      this.selectedPostId = postId;
      this.commentService.getByPost(postId).subscribe(comments => {
        this.postComments = comments;
        this.cdr.detectChanges();

      });
    }
  }

  addComment(): void {
    if(!this.isLoggedIn()) return;
    if (!this.newCommentBody.trim() || this.selectedPostId === null || this.currentUserId === null) return;

    const newComment: CreateComment = {
      postId: this.selectedPostId,
      body: this.newCommentBody.trim(),
      clientId: this.role === 'CLIENT' ? this.currentUserId : null,
      serviceProviderId: this.role === 'SERVICEPROVIDER' ? this.currentUserId : null
    };
    this.commentService.create(newComment).subscribe(comment => {
      this.postComments.push(comment);
      this.newCommentBody = '';
      if (this.selectedPostId !== null) {
        this.commentCounts[this.selectedPostId] = (this.commentCounts[this.selectedPostId] || 0) + 1;
      }
      this.cdr.detectChanges();
    });
  }

  toggleLike(post: Post) {
  let userlike:IUserLikes
  if (post.isLikedByCurrentUser) {
    post.likes--;
    userlike = { postId:post.id , userId:this.tokenHandler.UserId , isliked:false}
    console.log(userlike)
  } else {
    post.likes++;
   userlike = { postId:post.id , userId:this.tokenHandler.UserId , isliked:true}

  }

  this.userLikesService.putUserLike(userlike).subscribe({
     next: () => console.log('User Like puted'),
    error: err => {

      console.error('Error puted like:', err);
    }
  });
  post.isLikedByCurrentUser = userlike.isliked;
  this.cdr.detectChanges()
  this.postService.update(post).subscribe({
    next: () => console.log('Like updated'),
    error: err => {
      post.isLikedByCurrentUser = !post.isLikedByCurrentUser;
      post.likes += post.isLikedByCurrentUser ? 1 : -1;
      console.error('Error updating like:', err);
    }
  });
}
}



