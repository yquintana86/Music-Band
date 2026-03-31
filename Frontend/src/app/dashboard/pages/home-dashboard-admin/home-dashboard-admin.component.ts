import { Component, computed, inject, OnInit, signal, untracked } from '@angular/core';
import { DashboardService } from '../../services/dashboard.service';
import { MostUsedInstrumentResponse, MusicianDashboardGenericsResponse } from '../../interfaces';
import { CurrencyPipe, DecimalPipe } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { ErrorUtilitiesClass } from '../../../shared/interfaces/error-utilities.class';

@Component({
  selector: 'app-general-dashboard-admin',
  imports: [ DecimalPipe, CurrencyPipe],
  templateUrl: './home-dashboard-admin.component.html',
  styleUrl: './home-dashboard-admin.component.css',
})
export default class HomeDashboardAdminComponent implements OnInit  {

  private readonly _dashboardService = inject(DashboardService);
  private readonly _toastrService = inject(ToastrService);
  public dashboardGenerics = signal<MusicianDashboardGenericsResponse | null>(null);
  public mostPlayedInstrument = signal<MostUsedInstrumentResponse | null>(null);
  public domesticSeniorMusicians = signal<number | null>(null);

  mostPlayedInstrumentComputed = computed(() => {
    if(!this.mostPlayedInstrument())
      return [];

    return Object.entries(this.mostPlayedInstrument()?.musiciansByInstrumentName!);
  })
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

  public getMostUsedInstrumentResponse(): void {
    this._dashboardService.getMostPlayedInstrument({InstrumentQtyToSearch: 3})
    .subscribe({
      next: (response) => {
        this.mostPlayedInstrument.set(response);
      },
      error: (err) => {
        console.log(err);
        this.mostPlayedInstrument.set(null);
        this._toastrService.error(ErrorUtilitiesClass.getErrorMessage(err), 'Error');
      }
    })
  }

  public getDomesticSeniorMusicians(): void{
    this._dashboardService.searchDomesticSeniorMusiciansAsync()
    .subscribe({
      next: (response) => {
        this.domesticSeniorMusicians.set(response);
      },
      error: (err) => {
        console.log(err);
        this.domesticSeniorMusicians.set(null);
        this._toastrService.error(ErrorUtilitiesClass.getErrorMessage(err), 'Error');
      }
    })
  }



}
