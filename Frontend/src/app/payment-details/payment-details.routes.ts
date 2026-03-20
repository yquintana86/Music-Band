import { Routes } from "@angular/router";

export default [
  {
    path: 'list',
    title: 'Payment Details',
    loadComponent: () => import('./pages/payment-list/payment-list.component')
  },
  {
    path: '**',
    redirectTo: ''
  }
] as Routes
