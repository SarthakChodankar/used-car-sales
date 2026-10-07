import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PagedResult } from '../../shared/models/paged-result';
import { Bill, BillQuery, SaleRequest } from './billing.model';

@Injectable({ providedIn: 'root' })
export class BillingService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl;

  createSale(body: SaleRequest): Observable<Bill> {
    return this.http.post<Bill>(`${this.apiUrl}/api/sales`, body);
  }

  getAll(query: BillQuery): Observable<PagedResult<Bill>> {
    let params = new HttpParams()
      .set('pageNumber', query.pageNumber)
      .set('pageSize', query.pageSize);
    if (query.search) params = params.set('search', query.search);
    return this.http.get<PagedResult<Bill>>(`${this.apiUrl}/api/bills`, { params });
  }

  getById(id: number): Observable<Bill> {
    return this.http.get<Bill>(`${this.apiUrl}/api/bills/${id}`);
  }
}