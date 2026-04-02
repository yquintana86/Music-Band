import { AfterViewInit, Component, computed, ElementRef, inject, OnInit, signal, untracked, ViewChild, viewChild } from '@angular/core';
import { DashboardService } from '../../services/dashboard.service';
import { MostUsedInstrumentResponse, MusicianDashboardGenericsResponse } from '../../interfaces';
import { CurrencyPipe, DecimalPipe, NgClass } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { ErrorUtilitiesClass } from '../../../shared/interfaces/error-utilities.class';
import { SelectItem } from '../../../shared/interfaces';
import { forkJoin, fromEvent, map } from 'rxjs';
import { MusicianResponse } from '../../../musician/interfaces';

@Component({
  selector: 'app-general-dashboard-admin',
  imports: [DecimalPipe, CurrencyPipe, NgClass],
  templateUrl: './home-dashboard-admin.component.html',
  styleUrl: './home-dashboard-admin.component.css',
})
export default class HomeDashboardAdminComponent implements OnInit, AfterViewInit {

  private readonly _dashboardService = inject(DashboardService);
  private readonly _toastrService = inject(ToastrService);
  public dashboardGenerics = signal<MusicianDashboardGenericsResponse | null>(null);
  public mostPlayedInstrument = signal<MostUsedInstrumentResponse | null>(null);
  public domesticSeniorMusicians = signal<number | null>(null);
  public selectableInstruments = signal<SelectItem[]>([]);
  public musiciansByInstrument = signal<MusicianResponse[]>([]);
  private instrumentByMusicianSelect = viewChild<ElementRef<HTMLSelectElement>>('instrumentByMusicianSelect');
  public instrumentByMusicianSelectValue = signal<string>('');

  mostPlayedInstrumentComputed = computed(() => {
    if (!this.mostPlayedInstrument())
      return [];

    return Object.entries(this.mostPlayedInstrument()?.musiciansByInstrumentName!);
  })

  ngAfterViewInit(): void {
    const instrumentByMusicianSelect = this.instrumentByMusicianSelect()?.nativeElement;

    if (instrumentByMusicianSelect) {
      fromEvent(instrumentByMusicianSelect!, 'change')
        .pipe(
          map(event => (event.target as HTMLSelectElement).value)
        )
        .subscribe((value) => {
          this.instrumentByMusicianSelectValue.set(value);
        });
    }
  }
  ngOnInit(): void {

    var disctinct = this._dashboardService.getDisctinctInstruments();
    var MusicianDashboardGenerics = this._dashboardService.getMusicianDashboardGenerics({ startDate: undefined, endDate: undefined })

    forkJoin([disctinct, MusicianDashboardGenerics]).subscribe({
      next: ([disctinctResponse, MusicianDashboardGenericsResponse]) => {
        this.dashboardGenerics.set(MusicianDashboardGenericsResponse);
        this.selectableInstruments.set(disctinctResponse);
      }, error: (err) => {
        console.log(err);
        this.dashboardGenerics.set(null);
        this._toastrService.error(ErrorUtilitiesClass.getErrorMessage(err), 'Error');
      }
    })
  }

  public getMostUsedInstrumentResponse(): void {
    this._dashboardService.getMostPlayedInstrument({ InstrumentQtyToSearch: 3 })
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

  public getDomesticSeniorMusicians(): void {
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


  getMusiciansByInstrument(instrumentId: string): void {
    const Nu = Number(instrumentId);
    if (!instrumentId || isNaN(Nu)) {
      this._toastrService.error('Please select a valid instrument', 'Error');
      return;
    }
    this._dashboardService.getMusicianByInstrument(Nu)
      .subscribe({
        next: (response) => {
          this.musiciansByInstrument.set(response);
        },
        error: (err) => {
          console.log(err);
          this.musiciansByInstrument.set([]);
          this._toastrService.error(ErrorUtilitiesClass.getErrorMessage(err), 'Error');
        }
      })

  }
}
