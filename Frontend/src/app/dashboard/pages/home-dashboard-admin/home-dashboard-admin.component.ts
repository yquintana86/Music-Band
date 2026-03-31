import { Component, inject, OnInit, signal, untracked } from '@angular/core';
import { DashboardService } from '../../services/dashboard.service';
import { MusicianDashboardGenericsResponse } from '../../interfaces';
import { CurrencyPipe, DecimalPipe } from '@angular/common';

@Component({
  selector: 'app-general-dashboard-admin',
  imports: [ DecimalPipe, CurrencyPipe],
  templateUrl: './home-dashboard-admin.component.html',
  styleUrl: './home-dashboard-admin.component.css',
})
export default class HomeDashboardAdminComponent implements OnInit  {

  private readonly _dashboardService = inject(DashboardService);
  public dashboardGenerics = signal<MusicianDashboardGenericsResponse | null>(null);

  ngOnInit(): void {
    this._dashboardService.getMusicianDashboardGenerics({startDate: undefined, endDate: undefined})
    .subscribe({
      next: (response) => {
        this.dashboardGenerics.set(response);
      },
      error: (err) => {
        console.log(err);
        this.dashboardGenerics.set(null);
      }
    })
  }

}
