import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { concatMap, from, map, Observable, toArray } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { apiErrorMessage } from '../../../shared/utils/api-error';
import { CarImage, CarSaveRequest } from '../car.model';
import { CarService } from '../car.service';

@Component({
  selector: 'app-car-form',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './car-form.html',
  styleUrl: './car-form.scss',
})
export class CarForm {
  private readonly fb = inject(FormBuilder);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly carService = inject(CarService);

  // /cars/new has no id, /cars/:id/edit has one
  readonly carId: number | null = Number(this.route.snapshot.paramMap.get('id')) || null;
  readonly isEdit = this.carId !== null;

  readonly images = signal<CarImage[]>([]);
  readonly pendingFiles = signal<File[]>([]);
  readonly saving = signal(false);
  readonly error = signal<string | null>(null);

  readonly fuelTypes = ['Petrol', 'Diesel', 'CNG', 'Electric', 'Hybrid'];
  readonly transmissions = ['Manual', 'Automatic'];

  readonly form = this.fb.nonNullable.group({
    vin: ['', [Validators.required, Validators.minLength(11), Validators.maxLength(17)]],
    make: ['', [Validators.required, Validators.maxLength(50)]],
    model: ['', [Validators.required, Validators.maxLength(50)]],
    manufactureYear: [new Date().getFullYear(), [Validators.required, Validators.min(1980), Validators.max(2100)]],
    mileage: [0, [Validators.required, Validators.min(0)]],
    color: ['', Validators.maxLength(30)],
    fuelType: ['Petrol', Validators.required],
    transmission: ['Manual', Validators.required],
    price: [0, [Validators.required, Validators.min(0.01)]],
    description: ['', Validators.maxLength(2000)],
  });

  constructor() {
    if (this.carId !== null) {
      this.carService.getById(this.carId).subscribe({
        next: (car) => {
          if (car.status === 'Sold') {
            this.error.set('A sold car cannot be edited.');
            this.form.disable();
          }
          this.form.patchValue({
            ...car,
            color: car.color ?? '',
            description: car.description ?? '',
          });
          this.images.set(car.images);
        },
        error: (e) => this.error.set(apiErrorMessage(e)),
      });
    }
  }

  invalid(name: string): boolean {
    const c = this.form.get(name);
    return !!c && c.invalid && (c.touched || c.dirty);
  }

  fullUrl(path: string): string {
    return `${environment.apiUrl}${path}`;
  }

  save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const v = this.form.getRawValue();
    const body: CarSaveRequest = {
      ...v,
      color: v.color.trim() || null,
      description: v.description.trim() || null,
    };

    this.saving.set(true);
    this.error.set(null);

    const id = this.carId;
    const request$: Observable<number> =
      id !== null
        ? this.carService.update(id, body).pipe(map(() => id))
        : this.carService.create(body).pipe(
            concatMap((car) =>
              this.uploadAll(car.carId, this.pendingFiles()).pipe(map(() => car.carId)),
            ),
          );

    request$.subscribe({
      next: (carId) => this.router.navigate(['/cars', carId]),
      error: (e) => {
        this.error.set(apiErrorMessage(e));
        this.saving.set(false);
      },
    });
  }

  onFiles(event: Event): void {
    const input = event.target as HTMLInputElement;
    const files = Array.from(input.files ?? []);
    input.value = '';
    if (files.length === 0) return;

    if (this.carId === null) {
      // New car: remember the files, upload them after the car is saved.
      this.pendingFiles.update((list) => [...list, ...files]);
      return;
    }

    const id = this.carId;
    this.error.set(null);
    this.uploadAll(id, files).subscribe({
      next: () => this.refreshImages(id),
      error: (e) => {
        this.error.set(apiErrorMessage(e));
        this.refreshImages(id);
      },
    });
  }

  makePrimary(img: CarImage): void {
    this.carService.setPrimaryImage(this.carId!, img.imageId).subscribe({
      next: () => this.refreshImages(this.carId!),
      error: (e) => this.error.set(apiErrorMessage(e)),
    });
  }

  removeImage(img: CarImage): void {
    if (!confirm('Remove this photo?')) return;
    this.carService.deleteImage(this.carId!, img.imageId).subscribe({
      next: () => this.refreshImages(this.carId!),
      error: (e) => this.error.set(apiErrorMessage(e)),
    });
  }

  private refreshImages(carId: number): void {
    this.carService.getById(carId).subscribe((car) => this.images.set(car.images));
  }

  // Uploads one file at a time so the first one becomes the main photo.
  private uploadAll(carId: number, files: File[]): Observable<unknown> {
    return from(files).pipe(
      concatMap((file) => this.carService.uploadImage(carId, file)),
      toArray(),
    );
  }
}