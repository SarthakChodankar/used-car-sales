import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { apiErrorMessage } from '../../../shared/utils/api-error';
import { CustomerSaveRequest } from '../customer.model';
import { CustomerService } from '../customer.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-customer-form',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './customer-form.html',
  styleUrl: './customer-form.scss',
})
export class CustomerForm {
  private readonly fb = inject(FormBuilder);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly service = inject(CustomerService);

  readonly customerId: number | null = Number(this.route.snapshot.paramMap.get('id')) || null;
  readonly isEdit = this.customerId !== null;

  readonly saving = signal(false);
  readonly error = signal<string | null>(null);

  readonly form = this.fb.nonNullable.group({
    fullName: ['', [Validators.required, Validators.maxLength(150)]],
    email: ['', [Validators.required, Validators.email, Validators.maxLength(150)]],
    phone: ['', [Validators.required, Validators.pattern(/^[0-9+\-\s]{7,20}$/)]],
    address: ['', Validators.maxLength(300)],
  });

  constructor() {
    if (this.customerId !== null) {
      this.service.getById(this.customerId).subscribe({
        next: (c) => this.form.patchValue({ ...c, address: c.address ?? '' }),
        error: (e) => this.error.set(apiErrorMessage(e)),
      });
    }
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

    const v = this.form.getRawValue();
    const body: CustomerSaveRequest = { ...v, address: v.address.trim() || null };

    this.saving.set(true);
    this.error.set(null);

   const request$: Observable<unknown> =
  this.customerId !== null
    ? this.service.update(this.customerId, body)
    : this.service.create(body);

    request$.subscribe({
      next: () => this.router.navigate(['/customers']),
      error: (e) => {
        this.error.set(apiErrorMessage(e));
        this.saving.set(false);
      },
    });
  }
}