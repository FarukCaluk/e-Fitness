import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Announcement, CreateAnnouncementRequest } from '../models/announcement.model';
import { AnnouncementSegment } from '../models/enums.model';
import { PaginatedList } from '../models/pagination.model';
import { buildHttpParams } from '../utils/http-params.util';

export interface AnnouncementsFilter {
  pageNumber?: number;
  pageSize?: number;
  segment?: AnnouncementSegment;
}

@Injectable({ providedIn: 'root' })
export class AnnouncementsService {
  private readonly baseUrl = `${environment.apiUrl}/announcements`;

  constructor(private readonly http: HttpClient) {}

  getAnnouncements(filter: AnnouncementsFilter): Observable<PaginatedList<Announcement>> {
    return this.http.get<PaginatedList<Announcement>>(this.baseUrl, { params: buildHttpParams(filter) });
  }

  createAnnouncement(request: CreateAnnouncementRequest): Observable<{ id: number }> {
    return this.http.post<{ id: number }>(this.baseUrl, request);
  }
}
