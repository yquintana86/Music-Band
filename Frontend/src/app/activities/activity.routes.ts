import { Routes } from "@angular/router";

export default [
  {
    path: 'list',
    title: 'List',
    loadComponent: () => import('./pages/activity-list/activity-list.component')
  },
  {
    path: 'details/:id',
    title: 'Details',
    loadComponent: () => import('./pages/activity-details/activity-details.component')
  },
  {
    path: '**',
    redirectTo: 'list'
  }
] as Routes
