import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { MembershipStatus } from '../models/enums.model';
import { Membership, SubscribeToMembershipRequest } from '../models/membership.model';
import { PaginatedList } from '../models/pagination.model';
import { buildHttpParams } from '../utils/http-params.util';

export interface MembershipsFilter {
  pageNumber?: number;
  pageSize?: number;
  memberId?: number;
  status?: MembershipStatus;
}

@Injectable({ providedIn: 'root' })
export class MembershipsService {
  private readonly baseUrl = `${environment.apiUrl}/memberships`;

  constructor(private readonly http: HttpClient) {}

  getMemberships(filter: MembershipsFilter): Observable<PaginatedList<Membership>> {
    return this.http.get<PaginatedList<Membership>>(this.baseUrl, { params: buildHttpParams(filter) });
  }

  subscribe(request: SubscribeToMembershipRequest): Observable<{ id: number }> {
    return this.http.post<{ id: number }>(this.baseUrl, request);
  }

  cancel(id: number): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/${id}/cancel`, {});
  }
}
