import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PaginatedList } from '../models/pagination.model';
import { CreateProgressLogRequest, ProgressLog } from '../models/progress-log.model';
import { buildHttpParams } from '../utils/http-params.util';

export interface ProgressLogsFilter {
  pageNumber?: number;
  pageSize?: number;
  memberId?: number;
}

@Injectable({ providedIn: 'root' })
export class ProgressLogsService {
  private readonly baseUrl = `${environment.apiUrl}/progress-logs`;

  constructor(private readonly http: HttpClient) {}

  getProgressLogs(filter: ProgressLogsFilter): Observable<PaginatedList<ProgressLog>> {
    return this.http.get<PaginatedList<ProgressLog>>(this.baseUrl, { params: buildHttpParams(filter) });
  }

  createProgressLog(request: CreateProgressLogRequest): Observable<{ id: number }> {
    return this.http.post<{ id: number }>(this.baseUrl, request);
  }
}
