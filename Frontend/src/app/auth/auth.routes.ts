import { Routes } from "@angular/router";
import AuthLayoutComponent from "./layout/auth-layout.component";
import { isNotAuthenticatedGuard } from "../core/guards/is-not-authenticated.guard";

export default [
{
  path: '',
  component: AuthLayoutComponent,
  children: [
    {
      path:'login',
      title:'Login',
      loadComponent:() => import('./pages/login-page/login-page.component')
    },
    {
      path: 'register',
      title: 'Register',
      loadComponent:() => import('./pages/register-page/register-page.component')
    },
    {
    path: 'forgot-password',
    title: 'Forgot Password',
    loadComponent: () => import('./pages/forgot-password-page/forgot-password-page.component')
    },
    {
      path: 'reset-forgotten-password',
      title: 'Reset Forgotten Password',
      loadComponent: () => import('./pages/reset-forgotten-password-page/reset-forgotten-password-page.component')
    },
    {
      path: '**',
      redirectTo: 'login',
    }
  ]
}
] as Routes
