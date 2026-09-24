export interface ConversationSummary {
  id: number;
  otherParticipantId: number;
  otherParticipantName: string;
  lastMessagePreview?: string | null;
  lastMessageAt?: string | null;
  unreadCount: number;
}

export interface ChatMessage {
  id: number;
  senderId: number;
  senderName: string;
  content: string;
  sentAt: string;
  isRead: boolean;
}
