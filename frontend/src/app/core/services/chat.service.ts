import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ChatMessage, ConversationSummary } from '../models/chat.model';
import { PaginatedList } from '../models/pagination.model';
import { buildHttpParams } from '../utils/http-params.util';

@Injectable({ providedIn: 'root' })
export class ChatService {
  private readonly baseUrl = `${environment.apiUrl}/chat`;

  constructor(private readonly http: HttpClient) {}

  getConversations(pageNumber = 1, pageSize = 20): Observable<PaginatedList<ConversationSummary>> {
    return this.http.get<PaginatedList<ConversationSummary>>(`${this.baseUrl}/conversations`, {
      params: buildHttpParams({ pageNumber, pageSize })
    });
  }

  startConversation(otherUserId: number): Observable<{ id: number }> {
    return this.http.post<{ id: number }>(`${this.baseUrl}/conversations`, { otherUserId });
  }

  getMessages(conversationId: number, pageNumber = 1, pageSize = 50): Observable<PaginatedList<ChatMessage>> {
    return this.http.get<PaginatedList<ChatMessage>>(`${this.baseUrl}/conversations/${conversationId}/messages`, {
      params: buildHttpParams({ pageNumber, pageSize })
    });
  }

  sendMessage(conversationId: number, content: string): Observable<{ id: number }> {
    return this.http.post<{ id: number }>(`${this.baseUrl}/conversations/${conversationId}/messages`, { content });
  }
}
