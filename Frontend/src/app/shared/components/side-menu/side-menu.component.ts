import { Component, inject, OnInit, output, WritableSignal } from '@angular/core';
import { User } from '../../../auth/interfaces';
import { AuthService } from '../../../core/services/auth.service';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ErrorUtilitiesClass } from '../../interfaces/error-utilities.class';

@Component({
  selector: 'app-side-menu',
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './side-menu.component.html',
  styleUrl: './side-menu.component.css',
})
export class SideMenuComponent {

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
