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

  public getMusicianDashboardSummary(query: MusicianDashboardGenericsQuery): Observable<MusicianDashboardGenericsResponse>
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

}
