import { Routes } from "@angular/router";

export default [
  {
    path: 'list',
    title: 'List',
    loadComponent: () => import('./pages/musician-list/musician-list.component')
  },
  {
    path: 'details/:id',
    title: 'Details',
    loadComponent: () => import('./pages/musician-details/musician-details.component')
  },
  {
    path: '**',
    redirectTo: 'list'
  }
] as Routes
