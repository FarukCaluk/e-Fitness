import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ProductCategory } from '../models/enums.model';
import { PaginatedList } from '../models/pagination.model';
import { CreateProductRequest, Product, UpdateProductRequest } from '../models/product.model';
import { buildHttpParams } from '../utils/http-params.util';

export interface ProductsFilter {
  pageNumber?: number;
  pageSize?: number;
  searchTerm?: string;
  category?: ProductCategory;
  isActive?: boolean;
}

@Injectable({ providedIn: 'root' })
export class ProductsService {
  private readonly baseUrl = `${environment.apiUrl}/products`;

  constructor(private readonly http: HttpClient) {}

  getProducts(filter: ProductsFilter): Observable<PaginatedList<Product>> {
    return this.http.get<PaginatedList<Product>>(this.baseUrl, { params: buildHttpParams(filter) });
  }

  createProduct(request: CreateProductRequest): Observable<{ id: number }> {
    return this.http.post<{ id: number }>(this.baseUrl, request);
  }

  updateProduct(id: number, request: UpdateProductRequest): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, request);
  }

  deleteProduct(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
