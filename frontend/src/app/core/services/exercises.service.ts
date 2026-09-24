import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ExerciseDifficulty, MuscleGroup } from '../models/enums.model';
import { CreateExerciseRequest, Exercise, UpdateExerciseRequest } from '../models/exercise.model';
import { PaginatedList } from '../models/pagination.model';
import { buildHttpParams } from '../utils/http-params.util';

export interface ExercisesFilter {
  pageNumber?: number;
  pageSize?: number;
  searchTerm?: string;
  muscleGroup?: MuscleGroup;
  difficulty?: ExerciseDifficulty;
}

@Injectable({ providedIn: 'root' })
export class ExercisesService {
  private readonly baseUrl = `${environment.apiUrl}/exercises`;

  constructor(private readonly http: HttpClient) {}

  getExercises(filter: ExercisesFilter): Observable<PaginatedList<Exercise>> {
    return this.http.get<PaginatedList<Exercise>>(this.baseUrl, { params: buildHttpParams(filter) });
  }

  createExercise(request: CreateExerciseRequest): Observable<{ id: number }> {
    return this.http.post<{ id: number }>(this.baseUrl, request);
  }

  updateExercise(id: number, request: UpdateExerciseRequest): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, request);
  }

  deleteExercise(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
