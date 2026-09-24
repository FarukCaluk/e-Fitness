import { NotificationType } from './enums.model';

export interface Notification {
  id: number;
  title: string;
  message: string;
  type: NotificationType;
  isRead: boolean;
  createdAt: string;
}
