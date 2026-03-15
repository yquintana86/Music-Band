import { Routes } from '@angular/router';

export default [
{
  path: 'list',
  title: 'List',
  loadComponent: () => import('./pages/instrument-list/instrument-list.component')
},
{
  path: '**',
  redirectTo: 'list'
}
] as Routes
