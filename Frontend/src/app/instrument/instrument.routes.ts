import { Routes } from '@angular/router';

export default [
{
  path: 'list',
  title: 'List',
  loadComponent: () => import('./pages/instrument-list/instrument-list.component')
},
{
  path: 'details/:id',
  title: 'Details',
  loadComponent: () => import('./pages/instrument-details/instrument-details.component')
},
{
  path: '**',
  redirectTo: 'list'
}
] as Routes
