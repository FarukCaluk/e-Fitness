import { AnnouncementSegment } from './enums.model';

export interface Announcement {
  id: number;
  title: string;
  body: string;
  segment: AnnouncementSegment;
  authorName: string;
  createdAt: string;
}

export interface CreateAnnouncementRequest {
  title: string;
  body: string;
  segment: AnnouncementSegment;
}
