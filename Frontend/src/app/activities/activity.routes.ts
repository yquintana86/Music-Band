import { Routes } from "@angular/router";

export default [
  {
    path: 'list',
    title: 'List',
    loadComponent: () => import('./pages/activity-list/activity-list.component')
  },
  {
    path: '**',
    redirectTo: 'list'
  }
] as Routes
