import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { catchError, map, Observable, tap, throwError } from 'rxjs';
import { APIOperationResult, PagedResult } from '../../shared/interfaces';
import { RangePlusResponse } from '../interfaces/range-plus-response.interface';

@Injectable({
  providedIn: 'root',
})
export class RangePlusService {

  private readonly _httpClient = inject(HttpClient)
  private readonly baseUrl: string = `${environment.API_BASE_URL}/api/rangeplus`;


  public getAllRangePlus(): Observable<RangePlusResponse[]>{
    const  uri = `${this.baseUrl}/list`;

    return this._httpClient.get<APIOperationResult<RangePlusResponse[]>>(uri)
    .pipe(
      map(response => response.data || []),
      catchError(err => throwError(() => err))
    )
  }
}
