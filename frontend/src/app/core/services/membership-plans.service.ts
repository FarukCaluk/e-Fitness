import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  CreateMembershipPlanRequest,
  MembershipPlan,
  UpdateMembershipPlanRequest
} from '../models/membership-plan.model';
import { PaginatedList } from '../models/pagination.model';
import { buildHttpParams } from '../utils/http-params.util';

export interface MembershipPlansFilter {
  pageNumber?: number;
  pageSize?: number;
  isActive?: boolean;
}

@Injectable({ providedIn: 'root' })
export class MembershipPlansService {
  private readonly baseUrl = `${environment.apiUrl}/membership-plans`;

  constructor(private readonly http: HttpClient) {}

  getPlans(filter: MembershipPlansFilter): Observable<PaginatedList<MembershipPlan>> {
    return this.http.get<PaginatedList<MembershipPlan>>(this.baseUrl, { params: buildHttpParams(filter) });
  }

  getPlan(id: number): Observable<MembershipPlan> {
    return this.http.get<MembershipPlan>(`${this.baseUrl}/${id}`);
  }

  createPlan(request: CreateMembershipPlanRequest): Observable<{ id: number }> {
    return this.http.post<{ id: number }>(this.baseUrl, request);
  }

  updatePlan(id: number, request: UpdateMembershipPlanRequest): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, request);
  }

  deletePlan(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
