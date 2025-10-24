import { ChangeDetectorRef, Component } from '@angular/core';
import { CreatePost } from '../../Models/Post/CreatePost';
import { PostService } from '../../Services/Post&CommentServices/PostService';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { TokenHandlerService } from '../../Services/Auth/token-handler-service';

@Component({
  selector: 'app-custom-add-post-container',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './custom-add-post-container.html',
  styleUrl: './custom-add-post-container.css'
})
export class CustomAddPostContainer {
  newPost: CreatePost = { body: '' };


  constructor(
    private postService: PostService,
    private cdr: ChangeDetectorRef,
    private tokenHandler: TokenHandlerService
  ) {}

  isLoggedIn(): boolean {
    return this.tokenHandler.UserId !== null && this.tokenHandler.Role !== null;
  }

  createPost() {
    if (!this.isLoggedIn()) return;

    const role = this.tokenHandler.Role;
    const id = this.tokenHandler.UserId;

    const postPayload: CreatePost = {
      body: this.newPost.body,
      clientId: role == 'CLIENT' ? id : null,
      serviceProviderId: role == 'SERVICEPROVIDER' ? id : null,
      imagePath:this.newPost.imagePath ||  undefined
    };

    this.postService.create(postPayload).subscribe(() => {
      this.newPost.body = '';
      this.cdr.detectChanges();
      window.location.reload();
    });
  }
}
