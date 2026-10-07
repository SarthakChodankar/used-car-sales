import { CurrencyPipe, DatePipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { apiErrorMessage } from '../../../shared/utils/api-error';
import { Bill } from '../billing.model';
import { BillingService } from '../billing.service';

@Component({
  selector: 'app-bill-detail',
  imports: [RouterLink, CurrencyPipe, DatePipe],
  templateUrl: './bill-detail.html',
  styleUrl: './bill-detail.scss',
})
export class BillDetail {
  private readonly route = inject(ActivatedRoute);
  private readonly service = inject(BillingService);

  readonly bill = signal<Bill | null>(null);
  readonly error = signal<string | null>(null);

  constructor() {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.service.getById(id).subscribe({
      next: (b) => this.bill.set(b),
      error: (e) => this.error.set(apiErrorMessage(e)),
    });
  }

  print(): void {
    window.print();
  }
}