import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { catchError, map, Observable, throwError } from 'rxjs';
import { APIOperationResult, APIOperationResultBase, PagedResult } from '../../shared/interfaces';
import { InstrumentFilterQuery, UpdateInstrumentCommand, InstrumentResponse, CreateInstrumentCommand } from '../interfaces';

@Injectable({
  providedIn: 'root',
})
export class IntrumentService {

  #baseUrl = `${environment.API_BASE_URL}/api/instrument`;
  _httpClient = inject(HttpClient);

  public searchInstrumentsByFilter(filter: InstrumentFilterQuery): Observable<PagedResult<InstrumentResponse>> {
    const uri = `${this.#baseUrl}/search`;

    return this._httpClient
    .post<APIOperationResult<PagedResult<InstrumentResponse>>>(uri, filter)
    .pipe(
      map(respone => respone.data!),
      catchError(err => throwError(() => err))
    );
  }

  public getInstrumentById(id: number): Observable<InstrumentResponse> {
    const uri = `${this.#baseUrl}/${id}`;

    return this._httpClient
    .get<APIOperationResult<InstrumentResponse>>(uri)
    .pipe(
      map(response => response.data!),
      catchError(err => throwError(() => err))
    )
  }


  public createInstrument(instrument: CreateInstrumentCommand): Observable<boolean>{
    const uri = `${this.#baseUrl}/create`;
debugger;
    return this._httpClient
    .post<APIOperationResultBase>(uri, instrument)
    .pipe(
      map(response => response.isSuccess),
      catchError(err => throwError(() => err))
    )
  }

  public updateInstrument(instrument: UpdateInstrumentCommand): Observable<boolean>{
    const uri = `${this.#baseUrl}/update`;

    return this._httpClient
    .put<APIOperationResultBase>(uri, instrument)
    .pipe(
      map(response => response.isSuccess),
      catchError(err => throwError(() => err))
    )
  }

  public deleteInstrument(id: number): Observable<boolean>{
    const uri = `${this.#baseUrl}/${id}`;

    return this._httpClient
    .delete<APIOperationResultBase>(uri)
    .pipe(
      map(response => response.isSuccess),
      catchError(err => throwError(() => err))
    )
  }


  public deteleManyInstruments(ids: number[]): Observable<boolean> {
    const uri = `${this.#baseUrl}/deletemany`;

    return this._httpClient.post<APIOperationResultBase>(uri, ids)
    .pipe(
      map(response => response.isSuccess),
      catchError(err => throwError(() => err))
    )
  }
}
