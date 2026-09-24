import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { OrderStatus } from '../models/enums.model';
import { CreateOrderRequest, Order } from '../models/order.model';
import { PaginatedList } from '../models/pagination.model';
import { buildHttpParams } from '../utils/http-params.util';

export interface OrdersFilter {
  pageNumber?: number;
  pageSize?: number;
  memberId?: number;
  status?: OrderStatus;
}

@Injectable({ providedIn: 'root' })
export class OrdersService {
  private readonly baseUrl = `${environment.apiUrl}/orders`;

  constructor(private readonly http: HttpClient) {}

  getOrders(filter: OrdersFilter): Observable<PaginatedList<Order>> {
    return this.http.get<PaginatedList<Order>>(this.baseUrl, { params: buildHttpParams(filter) });
  }

  createOrder(request: CreateOrderRequest): Observable<{ id: number }> {
    return this.http.post<{ id: number }>(this.baseUrl, request);
  }

  updateStatus(id: number, status: OrderStatus): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}/status`, { status });
  }
}
