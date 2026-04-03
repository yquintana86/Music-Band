import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { catchError, map, Observable, pipe, throwError } from 'rxjs';
import { APIOperationResult } from '../../shared/interfaces';
import { GetMostUsedInstrumentQuery, MostUsedInstrumentResponse, MusicianDashboardGenericsQuery, MusicianDashboardGenericsResponse, SearchDomesticSeniorMusiciansQuery } from '../interfaces';
import { SelectItem } from '../../shared/interfaces/dto-with-id.interface';
import { MusicianResponse } from '../../musician/interfaces';

@Injectable({providedIn: 'root'})
export class DashboardService {

  private readonly _httpClient = inject(HttpClient);
  private readonly _baseUrl: string = `${environment.API_BASE_URL}/api`;

  public getMusicianDashboardGenerics(query: MusicianDashboardGenericsQuery): Observable<MusicianDashboardGenericsResponse>
  {
    const uri = `${this._baseUrl}/musician/dashboardgenerics`;
    debugger;
    return this._httpClient.post<APIOperationResult<MusicianDashboardGenericsResponse>>(uri, query)
    .pipe(
      map(response => response.data!),
      catchError(err => {
        console.log(err);
        return throwError(() => err);
      })
    );
  }

  public getMostPlayedInstrument(query: GetMostUsedInstrumentQuery = {InstrumentQtyToSearch: 3}): Observable<MostUsedInstrumentResponse>
  {
    const uri = `${this._baseUrl}/instrument/mostplayed/`;

    return this._httpClient.post<APIOperationResult<MostUsedInstrumentResponse>>(uri, query)
    .pipe(
      map(response => response.data!),
      catchError(err => {
        console.log(err);
        return throwError(() => err);
      })
    );
  }

  public searchDomesticSeniorMusiciansAsync(query: SearchDomesticSeniorMusiciansQuery = {age: 30}): Observable<number>
  {
    const uri = `${this._baseUrl}/musician/domesticbyage`;

    return this._httpClient.post<APIOperationResult<number>>(uri, query)
    .pipe(
      map(response => response.data!),
      catchError(err => {
        console.log(err);
        return throwError(() => err);
      })
    );
  }

  public getDisctinctInstruments(): Observable<SelectItem[]>
  {
    const uri = `${this._baseUrl}/instrument/disctinct`

    return this._httpClient.get<APIOperationResult<SelectItem[]>>(uri)
    .pipe(
      map(response => response.data!),
      catchError(err => throwError(() => err))
    );
  }

  public getMusicianByInstrument(instrumentId: number): Observable<MusicianResponse[]>
  {
    const uri = `${this._baseUrl}/instrument/musiciansbyinstrument`;

    return this._httpClient.post<APIOperationResult<MusicianResponse[]>>(uri, instrumentId)
    .pipe(
      map(response => response.data!),
      catchError(err => throwError(() => err))
    )
  }

  public getInternationalActivitiesByMusician(id: number): Observable<number>
  {
    const uri = `${this._baseUrl}/musician/internationalqty/${id}`;

    return this._httpClient.get<APIOperationResult<number>>(uri)
    .pipe(
      map(response => response.data!),
      catchError(err => throwError(() => err))
    );

  }

}
