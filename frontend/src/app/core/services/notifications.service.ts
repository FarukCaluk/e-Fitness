import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Notification } from '../models/notification.model';
import { PaginatedList } from '../models/pagination.model';
import { buildHttpParams } from '../utils/http-params.util';

export interface NotificationsFilter {
  pageNumber?: number;
  pageSize?: number;
  isRead?: boolean;
}

@Injectable({ providedIn: 'root' })
export class NotificationsService {
  private readonly baseUrl = `${environment.apiUrl}/notifications`;

  constructor(private readonly http: HttpClient) {}

  getNotifications(filter: NotificationsFilter): Observable<PaginatedList<Notification>> {
    return this.http.get<PaginatedList<Notification>>(this.baseUrl, { params: buildHttpParams(filter) });
  }

  markAsRead(id: number): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}/read`, {});
  }
}
