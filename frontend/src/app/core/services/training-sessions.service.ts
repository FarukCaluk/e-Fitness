import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { TrainingSessionStatus } from '../models/enums.model';
import { PaginatedList } from '../models/pagination.model';
import { CreateTrainingSessionRequest, TrainingSession } from '../models/training-session.model';
import { buildHttpParams } from '../utils/http-params.util';

export interface TrainingSessionsFilter {
  pageNumber?: number;
  pageSize?: number;
  trainerId?: number;
  memberId?: number;
  status?: TrainingSessionStatus;
  fromDate?: string;
  toDate?: string;
}

@Injectable({ providedIn: 'root' })
export class TrainingSessionsService {
  private readonly baseUrl = `${environment.apiUrl}/training-sessions`;

  constructor(private readonly http: HttpClient) {}

  getSessions(filter: TrainingSessionsFilter): Observable<PaginatedList<TrainingSession>> {
    return this.http.get<PaginatedList<TrainingSession>>(this.baseUrl, { params: buildHttpParams(filter) });
  }

  createSession(request: CreateTrainingSessionRequest): Observable<{ id: number }> {
    return this.http.post<{ id: number }>(this.baseUrl, request);
  }

  updateStatus(id: number, status: TrainingSessionStatus): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}/status`, { status });
  }
}
