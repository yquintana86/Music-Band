import { Title } from "@angular/platform-browser";
import DashboardLayoutComponent from "./layout/dashboard-layout.component";
import { Routes } from "@angular/router";

export default [
  {
    path: '',
    children: [
      {
        path: 'home',
        title: 'Admin',
        loadComponent: () => import('./pages/home-dashboard-admin/home-dashboard-admin.component')
      },
      {
        path: 'user-admin',
        title: 'Admin',
        loadComponent: () => import('./pages/user-admin-page/user-admin-page.component')
      },
      {
        path: 'payment-admin',
        title: 'Admin',
        loadComponent: () => import('./pages/payment-admin-page/payment-admin-page.component')
      },
      {
        path: 'musician-admin',
        title: 'Admin',
        loadComponent: () => import('./pages/musician-admin-page/musician-admin-page.component')
      },
      {
        path: 'activity-admin',
        title: 'Admin',
        loadComponent: () => import('./pages/activity-admin-page/activity-admin-page.component')
      },
      {
        path: '**',
        redirectTo: 'home'
      }
    ]
  }
] as Routes
