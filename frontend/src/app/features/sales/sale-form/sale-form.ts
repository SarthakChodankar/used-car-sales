import { CurrencyPipe } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { apiErrorMessage } from '../../../shared/utils/api-error';
import { BillingService } from '../../billing/billing.service';
import { Car } from '../../cars/car.model';
import { CarService } from '../../cars/car.service';
import { Customer } from '../../customers/customer.model';
import { CustomerService } from '../../customers/customer.service';

@Component({
  selector: 'app-sale-form',
  imports: [ReactiveFormsModule, RouterLink, CurrencyPipe],
  templateUrl: './sale-form.html',
  styleUrl: './sale-form.scss',
})
export class SaleForm {
  private readonly fb = inject(FormBuilder);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly carService = inject(CarService);
  private readonly customerService = inject(CustomerService);
  private readonly billingService = inject(BillingService);

  // Preview only. The API calculates the real tax, so the bill is always correct.
  readonly taxPercent = 18;
  readonly paymentMethods = ['Cash', 'Card', 'BankTransfer', 'Finance'];

  readonly cars = signal<Car[]>([]);
  readonly customers = signal<Customer[]>([]);
  readonly saving = signal(false);
  readonly error = signal<string | null>(null);

  readonly form = this.fb.nonNullable.group({
    carId: [0, [Validators.min(1)]],
    customerId: [0, [Validators.min(1)]],
    salePrice: [0, [Validators.required, Validators.min(0.01)]],
    paymentMethod: ['Cash', Validators.required],
  });

  private readonly price = toSignal(this.form.controls.salePrice.valueChanges, { initialValue: 0 });
  readonly tax = computed(() => Math.round((this.price() ?? 0) * this.taxPercent) / 100);
  readonly total = computed(() => (this.price() ?? 0) + this.tax());

  constructor() {
    // When a car is chosen, fill its listed price (the salesperson can still change it).
    this.form.controls.carId.valueChanges.subscribe((id) => {
      const car = this.cars().find((c) => c.carId === id);
      if (car) this.form.controls.salePrice.setValue(car.price);
    });

    this.carService.getAll({ status: 'Available', pageNumber: 1, pageSize: 50 }).subscribe({
      next: (r) => {
        this.cars.set(r.items);
        const preselected = Number(this.route.snapshot.queryParamMap.get('carId'));
        if (preselected && r.items.some((c) => c.carId === preselected)) {
          this.form.controls.carId.setValue(preselected);
        }
      },
      error: (e) => this.error.set(apiErrorMessage(e)),
    });

    this.customerService.getAll({ pageNumber: 1, pageSize: 50 }).subscribe({
      next: (r) => this.customers.set(r.items),
      error: (e) => this.error.set(apiErrorMessage(e)),
    });
  }

  invalid(name: string): boolean {
    const c = this.form.get(name);
    return !!c && c.invalid && (c.touched || c.dirty);
  }

  save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving.set(true);
    this.error.set(null);

    this.billingService.createSale(this.form.getRawValue()).subscribe({
      next: (bill) => this.router.navigate(['/bills', bill.billId]),
      error: (e) => {
        this.error.set(apiErrorMessage(e));
        this.saving.set(false);
      },
    });
  }
}