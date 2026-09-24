import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { EquipmentCategory } from '../models/enums.model';
import { CreateEquipmentRequest, Equipment, UpdateEquipmentRequest } from '../models/equipment.model';
import { PaginatedList } from '../models/pagination.model';
import { buildHttpParams } from '../utils/http-params.util';

export interface EquipmentFilter {
  pageNumber?: number;
  pageSize?: number;
  searchTerm?: string;
  category?: EquipmentCategory;
}

@Injectable({ providedIn: 'root' })
export class EquipmentService {
  private readonly baseUrl = `${environment.apiUrl}/equipment`;

  constructor(private readonly http: HttpClient) {}

  getEquipment(filter: EquipmentFilter): Observable<PaginatedList<Equipment>> {
    return this.http.get<PaginatedList<Equipment>>(this.baseUrl, { params: buildHttpParams(filter) });
  }

  createEquipment(request: CreateEquipmentRequest): Observable<{ id: number }> {
    return this.http.post<{ id: number }>(this.baseUrl, request);
  }

  updateEquipment(id: number, request: UpdateEquipmentRequest): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, request);
  }

  deleteEquipment(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
