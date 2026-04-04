import { AfterViewInit, Component, computed, ElementRef, inject, OnInit, signal, viewChild } from '@angular/core';
import { CurrencyPipe, DecimalPipe, NgClass } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { forkJoin, fromEvent, map } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

import { DashboardService } from '../../services/dashboard.service';
import { MostUsedInstrumentResponse, MusicianDashboardGenericsResponse } from '../../interfaces';
import { ErrorUtilitiesClass } from '../../../shared/interfaces/error-utilities.class';
import { DtoWithId, SelectItem } from '../../../shared/interfaces';
import { MusicianResponse, SearchMusicianByFilterQuery } from '../../../musician/interfaces';
import { InputSearchComponent } from "../../../shared/components/input-search/input-search.component";
import { MusicianService } from '../../../musician/services/musician.service';
import { InputSearchSelectorComponent } from "../../../shared/components/input-search-selector/input-search-selector.component";
import { environment } from '../../../../environments/environment.development';
import { InstrumentService } from '../../../instrument/services/instrument.service';

@Component({
  selector: 'app-general-dashboard-admin',
  imports: [DecimalPipe, CurrencyPipe, NgClass, InputSearchSelectorComponent, FormsModule],
  templateUrl: './home-dashboard-admin.component.html',
  styleUrl: './home-dashboard-admin.component.css',
})
export default class HomeDashboardAdminComponent implements OnInit, AfterViewInit {

  private readonly _dashboardService = inject(DashboardService);
  private readonly _musicianService = inject(MusicianService);
  private readonly _instrumentService = inject(InstrumentService);
  private readonly _toastrService = inject(ToastrService);
  public dashboardGenerics = signal<MusicianDashboardGenericsResponse | null>(null);
  public mostPlayedInstrument = signal<MostUsedInstrumentResponse | null>(null);
  public domesticSeniorMusicians = signal<number | null>(null);
  public selectableInstruments = signal<SelectItem[]>([]);
  public musiciansByInstrument = signal<MusicianResponse[]>([]);
  private instrumentByMusicianSelect = viewChild<ElementRef<HTMLSelectElement>>('instrumentByMusicianSelect');
  private inputSearch = viewChild<InputSearchComponent>('inputSearch');
  public instrumentByMusicianSelectValue = signal<string>('');
  public musiciansFounded = signal<SelectItem[]>([]);
  public internationalMusicianId = signal<number | null>(null);
  public InternationalActivitiesByMusician = signal<number | null>(null);

  public averageInstrument1 = signal<number | null>(null);
  public averageInstrument2 = signal<number | null>(null);
  public averageInstrument3 = signal<number | null>(null);
  public averageInstruments = signal<number | null>(null);


  public initialDto = signal<SelectItem | null>(null);
    public callBackShowFieldValueFn = (item: DtoWithId) => ({
      id: item.id,
      text: `${(item as MusicianResponse).firstName} ${(item as MusicianResponse).lastName}`,
    });

    public searchPostUri = `${environment.API_BASE_URL}/api/musician/search`;

    public callBackGetFilterFn = (query: string) => ({
    page: 1,
    pageSize: 10,
    firstName: query,
  }) as SearchMusicianByFilterQuery;

  public setMusicianId(musicianId: number | null) {
     this.internationalMusicianId.set(musicianId);
     if(musicianId === null){
       this.InternationalActivitiesByMusician.set(null);
     }
  }


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

    var disctinct = this._instrumentService.getDisctinctInstruments();
    var MusicianDashboardGenerics = this._dashboardService.getMusicianDashboardSummary({ startDate: undefined, endDate: undefined })

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

  public setAverageInstrument1(id: string | null) {
    this.averageInstrument1.set(!id ? null : parseInt(id));
  }
  public setAverageInstrument2(id: string | null) {
    this.averageInstrument2.set(!id ? null : parseInt(id));
  }
  public setAverageInstrument3(id: string | null) {
    this.averageInstrument3.set(!id ? null : parseInt(id));
  }

  public getAverageMusiciansByInstrument()
  {
    if(!this.averageInstrument1() && !this.averageInstrument2() && !this.averageInstrument3())
    {
      this._toastrService.error('Please select an instrument', 'Error');
      this.averageInstruments.set(null);
      return;
    }

    const averages = [this.averageInstrument1(), this.averageInstrument2(), this.averageInstrument3()];

    this._musicianService.getMusicianAverageByInstruments(averages.filter(id => !!id) as number[])
    .subscribe({
      next: (response) => {
        this.averageInstruments.set(response);
      },
      error: (err) => {
        console.log(err);
        this.averageInstruments.set(null);
        this._toastrService.error(ErrorUtilitiesClass.getErrorMessage(err), 'Error');
      }
    })

  }


  public getInternationalParticipationByMusician(): void{

    const musicianId = this.internationalMusicianId();
    if(!musicianId)
    {
      this._toastrService.error('Please select a musician', 'Error');
      this.InternationalActivitiesByMusician.set(null);
      return;
    }

    this._musicianService.getInternationalActivitiesByMusician(musicianId)
      .subscribe({
        next: (response) => {
          this.InternationalActivitiesByMusician.set(response);
        },
        error: (err) => {
          console.log(err);
          this.InternationalActivitiesByMusician.set(null);
          this._toastrService.error(ErrorUtilitiesClass.getErrorMessage(err), 'Error');
        }
      })
  }

  public getMostUsedInstrumentResponse(): void {
    this._instrumentService.getMostPlayedInstrument({ InstrumentQtyToSearch: 3 })
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
    this._musicianService.searchDomesticSeniorMusiciansAsync()
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
    this._instrumentService.getMusicianByInstrument(Nu)
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



   public dialogModalCleanClicked() {
      this.musiciansFounded.set([]);
      this.inputSearch()?.clearSelection();
    }
}
