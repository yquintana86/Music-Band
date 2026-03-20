import { Routes } from '@angular/router';
import { isAuthenticatedGuard } from './core/guards/is-authenticated.guard';
import { isNotAuthenticatedGuard } from './core/guards/is-not-authenticated.guard';
import DashboardLayoutComponent from './dashboard/layout/dashboard-layout.component';


export const routes: Routes = [
  {
    path: 'auth',
    canMatch: [isNotAuthenticatedGuard],
    loadChildren: () => import('./auth/auth.routes'),
  },
  {
    path: '',
    canMatch: [isAuthenticatedGuard],
    component: DashboardLayoutComponent,
    children: [
      {
        path: 'dashboard',
        loadChildren: () => import('./dashboard/dashboard.routes'),
      },
      {
        path: 'musician',
        canMatch: [isAuthenticatedGuard],
        loadChildren: () => import('./musician/musician.routes'),
      },
      {
        path: 'instrument',
        canMatch: [isAuthenticatedGuard],
        loadChildren: () => import('./instrument/instrument.routes')
      },
      {
        path: 'activity',
        loadChildren: () => import('./activities/activity.routes')
      },
      {
        path: 'payment',
        loadChildren: () => import('./payment-details/payment-details.routes')
      },
      {
        path: '**',
        redirectTo: 'dashboard'
      }
    ]
  },
  {
    path: '**',
    redirectTo: 'auth'
  }
];
