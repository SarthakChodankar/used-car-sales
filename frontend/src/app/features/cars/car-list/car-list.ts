import { CurrencyPipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { environment } from '../../../../environments/environment';
import { PagedResult } from '../../../shared/models/paged-result';
import { Car } from '../car.model';
import { CarService } from '../car.service';
import { RouterLink } from '@angular/router';
@Component({
  selector: 'app-car-list',
  imports: [FormsModule, CurrencyPipe, RouterLink],
  templateUrl: './car-list.html',
  styleUrl: './car-list.scss',
})
export class CarList {
  private readonly carService = inject(CarService);

  readonly result = signal<PagedResult<Car> | null>(null);
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);

  search = '';
  status = '';
  pageNumber = 1;
  readonly pageSize = 8;

  constructor() {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.error.set(null);

    this.carService
      .getAll({
        search: this.search.trim(),
        status: this.status,
        pageNumber: this.pageNumber,
        pageSize: this.pageSize,
      })
      .subscribe({
        next: (data) => {
          this.result.set(data);
          this.loading.set(false);
        },
        error: () => {
          this.error.set('Could not load cars. Please check that the API is running.');
          this.loading.set(false);
        },
      });
  }

  applyFilters(): void {
    this.pageNumber = 1;
    this.load();
  }

  goTo(page: number): void {
    this.pageNumber = page;
    this.load();
  }

  imageUrl(car: Car): string | null {
    return car.primaryImageUrl ? `${environment.apiUrl}${car.primaryImageUrl}` : null;
  }
}