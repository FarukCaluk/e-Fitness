import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PaymentStatus } from '../models/enums.model';
import { PaginatedList } from '../models/pagination.model';
import { Payment } from '../models/payment.model';
import { buildHttpParams } from '../utils/http-params.util';

export interface PaymentsFilter {
  pageNumber?: number;
  pageSize?: number;
  memberId?: number;
  status?: PaymentStatus;
  fromDate?: string;
  toDate?: string;
}

@Injectable({ providedIn: 'root' })
export class PaymentsService {
  private readonly baseUrl = `${environment.apiUrl}/payments`;

  constructor(private readonly http: HttpClient) {}

  getPayments(filter: PaymentsFilter): Observable<PaginatedList<Payment>> {
    return this.http.get<PaginatedList<Payment>>(this.baseUrl, { params: buildHttpParams(filter) });
  }
}
