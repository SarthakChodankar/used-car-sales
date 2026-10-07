import { Component, input, output } from '@angular/core';

@Component({
  selector: 'app-pager',
  template: `
    <nav class="pager" aria-label="Pagination">
      <button type="button" class="btn" (click)="pageChange.emit(pageNumber() - 1)"
              [disabled]="pageNumber() <= 1">Previous</button>
      <span>Page {{ pageNumber() }} of {{ totalPages() || 1 }} ({{ totalCount() }} total)</span>
      <button type="button" class="btn" (click)="pageChange.emit(pageNumber() + 1)"
              [disabled]="pageNumber() >= totalPages()">Next</button>
    </nav>
  `,
  styles: `.pager { display: flex; justify-content: center; align-items: center; gap: 1rem; margin-top: 1.25rem; }`,
})
export class Pager {
  readonly pageNumber = input.required<number>();
  readonly totalPages = input.required<number>();
  readonly totalCount = input.required<number>();
  readonly pageChange = output<number>();
}