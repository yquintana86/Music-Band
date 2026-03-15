import { Routes } from "@angular/router";

export default [
  {
    path: 'list',
    title: 'List',
    loadComponent: () => import('./pages/musician-list/musician-list.component')
  },
  {
    path: '**',
    redirectTo: 'list'
  }
] as Routes
