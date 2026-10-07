import { CurrencyPipe, DatePipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { Pager } from '../../../shared/components/pager';
import { PagedResult } from '../../../shared/models/paged-result';
import { apiErrorMessage } from '../../../shared/utils/api-error';
import { Bill } from '../billing.model';
import { BillingService } from '../billing.service';

@Component({
  selector: 'app-bill-list',
  imports: [FormsModule, RouterLink, CurrencyPipe, DatePipe, Pager],
  templateUrl: './bill-list.html',
  styleUrl: './bill-list.scss',
})
export class BillList {
  private readonly service = inject(BillingService);

  readonly result = signal<PagedResult<Bill> | null>(null);
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
}