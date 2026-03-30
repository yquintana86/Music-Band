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
        path: '**',
        redirectTo: 'home'
      }
    ]
  }
] as Routes
