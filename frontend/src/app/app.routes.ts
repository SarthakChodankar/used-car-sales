import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'cars' },

  { path: 'cars', loadComponent: () => import('./features/cars/car-list/car-list').then((m) => m.CarList) },
  { path: 'cars/new', loadComponent: () => import('./features/cars/car-form/car-form').then((m) => m.CarForm) },
  { path: 'cars/:id/edit', loadComponent: () => import('./features/cars/car-form/car-form').then((m) => m.CarForm) },
  { path: 'cars/:id', loadComponent: () => import('./features/cars/car-detail/car-detail').then((m) => m.CarDetail) },

  { path: 'customers', loadComponent: () => import('./features/customers/customer-list/customer-list').then((m) => m.CustomerList) },
  { path: 'customers/new', loadComponent: () => import('./features/customers/customer-form/customer-form').then((m) => m.CustomerForm) },
  { path: 'customers/:id/edit', loadComponent: () => import('./features/customers/customer-form/customer-form').then((m) => m.CustomerForm) },

  { path: 'sales/new', loadComponent: () => import('./features/sales/sale-form/sale-form').then((m) => m.SaleForm) },

  { path: 'bills', loadComponent: () => import('./features/billing/bill-list/bill-list').then((m) => m.BillList) },
  { path: 'bills/:id', loadComponent: () => import('./features/billing/bill-detail/bill-detail').then((m) => m.BillDetail) },

  { path: '**', redirectTo: 'cars' },
];