import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PaginatedList } from '../models/pagination.model';
import {
  CreateWorkoutPlanRequest,
  UpdateWorkoutPlanRequest,
  WorkoutPlanDetail,
  WorkoutPlanListItem
} from '../models/workout-plan.model';
import { buildHttpParams } from '../utils/http-params.util';

export interface WorkoutPlansFilter {
  pageNumber?: number;
  pageSize?: number;
  trainerId?: number;
  memberId?: number;
  isActive?: boolean;
}

@Injectable({ providedIn: 'root' })
export class WorkoutPlansService {
  private readonly baseUrl = `${environment.apiUrl}/workout-plans`;

  constructor(private readonly http: HttpClient) {}

  getPlans(filter: WorkoutPlansFilter): Observable<PaginatedList<WorkoutPlanListItem>> {
    return this.http.get<PaginatedList<WorkoutPlanListItem>>(this.baseUrl, { params: buildHttpParams(filter) });
  }

  getPlan(id: number): Observable<WorkoutPlanDetail> {
    return this.http.get<WorkoutPlanDetail>(`${this.baseUrl}/${id}`);
  }

  createPlan(request: CreateWorkoutPlanRequest): Observable<{ id: number }> {
    return this.http.post<{ id: number }>(this.baseUrl, request);
  }

  updatePlan(id: number, request: UpdateWorkoutPlanRequest): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, request);
  }

  deletePlan(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
