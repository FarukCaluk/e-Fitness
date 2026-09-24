import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { MembershipStatus } from '../models/enums.model';
import { MemberDetail, MemberListItem, UpdateMemberRequest } from '../models/member.model';
import { PaginatedList } from '../models/pagination.model';
import { buildHttpParams } from '../utils/http-params.util';

export interface MembersFilter {
  pageNumber?: number;
  pageSize?: number;
  searchTerm?: string;
  membershipStatus?: MembershipStatus;
}

@Injectable({ providedIn: 'root' })
export class MembersService {
  private readonly baseUrl = `${environment.apiUrl}/members`;

  constructor(private readonly http: HttpClient) {}

  getMembers(filter: MembersFilter): Observable<PaginatedList<MemberListItem>> {
    return this.http.get<PaginatedList<MemberListItem>>(this.baseUrl, { params: buildHttpParams(filter) });
  }

  getMember(id: number): Observable<MemberDetail> {
    return this.http.get<MemberDetail>(`${this.baseUrl}/${id}`);
  }

  getMyProfile(): Observable<MemberDetail> {
    return this.http.get<MemberDetail>(`${this.baseUrl}/me`);
  }

  getMyClients(filter: { pageNumber?: number; pageSize?: number; searchTerm?: string }): Observable<PaginatedList<MemberListItem>> {
    return this.http.get<PaginatedList<MemberListItem>>(`${this.baseUrl}/my-clients`, { params: buildHttpParams(filter) });
  }

  updateMember(id: number, request: UpdateMemberRequest): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, request);
  }
}
