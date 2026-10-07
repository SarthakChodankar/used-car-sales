import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { PagedResult } from '../../../shared/models/paged-result';
import { Pager } from '../../../shared/components/pager';
import { apiErrorMessage } from '../../../shared/utils/api-error';
import { Customer } from '../customer.model';
import { CustomerService } from '../customer.service';

@Component({
  selector: 'app-customer-list',
  imports: [FormsModule, RouterLink, Pager],
  templateUrl: './customer-list.html',
  styleUrl: './customer-list.scss',
})
export class CustomerList {
  private readonly service = inject(CustomerService);

  readonly result = signal<PagedResult<Customer> | null>(null);
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);

  search = '';
  pageNumber = 1;
  readonly pageSize = 10;

  constructor() {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.service
      .getAll({ search: this.search.trim(), pageNumber: this.pageNumber, pageSize: this.pageSize })
      .subscribe({
        next: (data) => {
          this.result.set(data);
          this.loading.set(false);
        },
        error: (e) => {
          this.error.set(apiErrorMessage(e));
          this.loading.set(false);
        },
      });
  }

  applySearch(): void {
    this.pageNumber = 1;
    this.load();
  }

  goTo(page: number): void {
    this.pageNumber = page;
    this.load();
  }

  remove(customer: Customer): void {
    if (!confirm(`Delete ${customer.fullName}?`)) return;
    this.error.set(null);
    this.service.delete(customer.customerId).subscribe({
      next: () => this.load(),
      error: (e) => this.error.set(apiErrorMessage(e)),
    });
  }
}