import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PagedResult } from '../../shared/models/paged-result';
import { Car, CarImage, CarQuery, CarSaveRequest } from './car.model';

@Injectable({ providedIn: 'root' })
export class CarService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/api/cars`;

  getAll(query: CarQuery): Observable<PagedResult<Car>> {
    let params = new HttpParams()
      .set('pageNumber', query.pageNumber)
      .set('pageSize', query.pageSize);

    if (query.search) params = params.set('search', query.search);
    if (query.status) params = params.set('status', query.status);

    return this.http.get<PagedResult<Car>>(this.baseUrl, { params });
  }

  getById(id: number): Observable<Car> {
    return this.http.get<Car>(`${this.baseUrl}/${id}`);
  }

  create(body: CarSaveRequest): Observable<Car> {
    return this.http.post<Car>(this.baseUrl, body);
  }

  update(id: number, body: CarSaveRequest): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, body);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  uploadImage(carId: number, file: File): Observable<CarImage> {
    const form = new FormData();
    form.append('file', file);
    return this.http.post<CarImage>(`${this.baseUrl}/${carId}/images`, form);
  }

  setPrimaryImage(carId: number, imageId: number): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${carId}/images/${imageId}/primary`, {});
  }

  deleteImage(carId: number, imageId: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${carId}/images/${imageId}`);
  }
}