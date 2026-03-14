import { Component, inject, input } from '@angular/core';
import { AuthService } from '../../../core/services/auth.service';
import { ErrorUtilitiesClass } from '../../../shared/interfaces/error-utilities.class';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { User } from '../../../auth/interfaces';

@Component({
  selector: 'app-header',
  imports: [],
  templateUrl: './header-nav.component.html',
  styleUrl: './header-nav.component.css',
})
export class HeaderNavComponent {

  headerTitle = input('Music Band');
  authService = inject(AuthService);
  router = inject(Router);
  toastr = inject(ToastrService);

  public get currentUser(): User | null {
      return this.authService.currentUser();
    }

  onLogout(): void {
    this.authService.logout().subscribe({
      next: () => {
        this.router.navigate(['/login']);
      },
      error: (err) => {
        this.toastr.error(ErrorUtilitiesClass.getErrorMessage(err), 'Error');}
      });
    }
  }
