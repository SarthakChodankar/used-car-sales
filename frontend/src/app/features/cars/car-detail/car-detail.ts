import { CurrencyPipe, DecimalPipe } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { environment } from '../../../../environments/environment';
import { Car } from '../car.model';
import { CarService } from '../car.service';

@Component({
  selector: 'app-car-detail',
  imports: [RouterLink, CurrencyPipe, DecimalPipe],
  templateUrl: './car-detail.html',
  styleUrl: './car-detail.scss',
})
export class CarDetail {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly carService = inject(CarService);

  readonly car = signal<Car | null>(null);
  readonly loading = signal(true);
  readonly error = signal<string | null>(null);
  readonly selectedIndex = signal(0);

  readonly selectedImage = computed(() => this.car()?.images[this.selectedIndex()] ?? null);

  private readonly carId = Number(this.route.snapshot.paramMap.get('id'));

  constructor() {
    this.carService.getById(this.carId).subscribe({
      next: (car) => {
        this.car.set(car);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('This car could not be found.');
        this.loading.set(false);
      },
    });
  }

  fullUrl(path: string): string {
    return `${environment.apiUrl}${path}`;
  }

  select(index: number): void {
    this.selectedIndex.set(index);
  }

  remove(): void {
    if (!confirm('Delete this car? This cannot be undone.')) return;

    this.carService.delete(this.carId).subscribe({
      next: () => this.router.navigate(['/cars']),
      error: (err) => this.error.set(err.error?.detail ?? 'Could not delete this car.'),
    });
  }
}