import { Component, OnInit } from '@angular/core';
import { ChatService } from '../../core/services/chat.service';
import { MembersService } from '../../core/services/members.service';
import { TrainersService } from '../../core/services/trainers.service';
import { AuthService } from '../../core/services/auth.service';
import { ConversationSummary, ChatMessage } from '../../core/models/chat.model';
import { UserRole } from '../../core/models/user-role.enum';

interface Contact {
  userId: number;
  name: string;
}

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.scss'
})
export class ChatComponent implements OnInit {
  conversations: ConversationSummary[] = [];
  messages: ChatMessage[] = [];
  contacts: Contact[] = [];
  selectedConversationId: number | null = null;
  newMessage = '';
  isLoadingConversations = true;
  isLoadingMessages = false;

  constructor(
    private readonly chatService: ChatService,
    private readonly membersService: MembersService,
    private readonly trainersService: TrainersService,
    private readonly authService: AuthService
  ) {}

  ngOnInit(): void {
    this.loadConversations();
    this.loadContacts();
  }

  loadConversations(): void {
    this.isLoadingConversations = true;
    this.chatService.getConversations(1, 50).subscribe((result) => {
      this.conversations = result.items;
      this.isLoadingConversations = false;
    });
  }

  loadContacts(): void {
    const role = this.authService.getCurrentUser()?.role;

    if (role === UserRole.Trainer) {
      this.membersService.getMyClients({ pageNumber: 1, pageSize: 100 }).subscribe((result) => {
        this.contacts = result.items.map((m) => ({ userId: m.userId, name: m.fullName }));
      });
      return;
    }

    if (role === UserRole.Client) {
      this.membersService.getMyProfile().subscribe((profile) => {
        if (!profile.assignedTrainerId) {
          return;
        }

        this.trainersService.getTrainer(profile.assignedTrainerId).subscribe((trainer) => {
          this.contacts = [{ userId: trainer.userId, name: `${trainer.firstName} ${trainer.lastName}` }];
        });
      });
    }
  }

  openConversation(conversationId: number): void {
    this.selectedConversationId = conversationId;
    this.isLoadingMessages = true;

    this.chatService.getMessages(conversationId, 1, 100).subscribe((result) => {
      this.messages = result.items;
      this.isLoadingMessages = false;
    });
  }

  startConversationWith(contact: Contact): void {
    this.chatService.startConversation(contact.userId).subscribe((result) => {
      this.loadConversations();
      this.openConversation(result.id);
    });
  }

  send(): void {
    if (!this.selectedConversationId || !this.newMessage.trim()) {
      return;
    }

    this.chatService.sendMessage(this.selectedConversationId, this.newMessage.trim()).subscribe(() => {
      this.newMessage = '';
      this.openConversation(this.selectedConversationId!);
      this.loadConversations();
    });
  }

  isMine(message: ChatMessage): boolean {
    return message.senderId === this.authService.getCurrentUser()?.id;
  }
}
