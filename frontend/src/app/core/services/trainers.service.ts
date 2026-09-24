import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CreateTrainerRequest, TrainerDetail, TrainerListItem, UpdateTrainerRequest } from '../models/trainer.model';
import { PaginatedList } from '../models/pagination.model';
import { buildHttpParams } from '../utils/http-params.util';

export interface TrainersFilter {
  pageNumber?: number;
  pageSize?: number;
  searchTerm?: string;
  isAvailable?: boolean;
}

@Injectable({ providedIn: 'root' })
export class TrainersService {
  private readonly baseUrl = `${environment.apiUrl}/trainers`;

  constructor(private readonly http: HttpClient) {}

  getTrainers(filter: TrainersFilter): Observable<PaginatedList<TrainerListItem>> {
    return this.http.get<PaginatedList<TrainerListItem>>(this.baseUrl, { params: buildHttpParams(filter) });
  }

  getTrainer(id: number): Observable<TrainerDetail> {
    return this.http.get<TrainerDetail>(`${this.baseUrl}/${id}`);
  }

  getMyProfile(): Observable<TrainerDetail> {
    return this.http.get<TrainerDetail>(`${this.baseUrl}/me`);
  }

  createTrainer(request: CreateTrainerRequest): Observable<{ id: number }> {
    return this.http.post<{ id: number }>(this.baseUrl, request);
  }

  updateTrainer(id: number, request: UpdateTrainerRequest): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, request);
  }

  deleteTrainer(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
