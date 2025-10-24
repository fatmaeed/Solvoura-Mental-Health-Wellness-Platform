import { ChangeDetectorRef, Component } from '@angular/core';
import { AIService } from '../../Services/AIService';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

interface Message {
  sender: 'user' | 'ai';
  content: string;
  timestamp: Date;
}

@Component({
  selector: 'app-custom-floating-action-btn',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './custom-floating-action-btn.html',
  styleUrls: ['./custom-floating-action-btn.css']
})
export class CustomFloatingActionBtn {
  prompt: string = '';
  isLoading: boolean = false;
  messages: Message[] = [];

  constructor(
    private aiService: AIService,
    private cdr: ChangeDetectorRef
  ) {}

  sendPrompt() {
    const trimmedPrompt = this.prompt.trim();
    if (!trimmedPrompt) return;
    this.messages.push({
      sender: 'user',
      content: trimmedPrompt,
      timestamp: new Date()
    });

    this.isLoading = true;
    this.prompt = '';

    this.aiService.getAIResponse(trimmedPrompt).subscribe({
      next: (res) => {
        this.messages.push({
          sender: 'ai',
          content: res.answer,
          timestamp: new Date()
        });
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error(err);
        this.messages.push({
          sender: 'ai',
          content: 'Something went wrong. Please try again later.',
          timestamp: new Date()
        });
        this.isLoading = false;
      }
    });
  }
}
