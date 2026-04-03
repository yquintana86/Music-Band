import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { catchError, map, Observable, throwError } from 'rxjs';

import { AverageByInstrumentsQuery, CreateMusicianCommand, MusicianResponse, SearchMusicianByFilterQuery, UpdateMusicianCommand } from '../interfaces';
import { APIOperationResult, APIOperationResultBase, PagedResult } from '../../shared/interfaces';
import { environment } from '../../../environments/environment.development';
import { SearchDomesticSeniorMusiciansQuery } from '../../dashboard/interfaces';


@Injectable({
  providedIn: 'root',
})
export class MusicianService {

  baseApiUrl = `${environment.API_BASE_URL}/api/musician`;
  #httpclient = inject(HttpClient);

//#region Queries
  public searchMusicianByQuery(query: SearchMusicianByFilterQuery): Observable<PagedResult<MusicianResponse>>{

    const searchUri = `${this.baseApiUrl}/search`;
    return this.#httpclient
    .post<APIOperationResult<PagedResult<MusicianResponse>>>(`${this.baseApiUrl}/search`, query)
    .pipe(
      map((response) => {
        if(!response.isSuccess)
          throw new Error(response.errors![0].message);
        return response.data;
      }),
      catchError((err) => throwError(() => err))
    );
  }

  public getById(id: number): Observable<MusicianResponse | null> {
    const searchUri = `${this.baseApiUrl}/${id}`;

    return this.#httpclient
    .get<APIOperationResult<MusicianResponse>>(searchUri)
    .pipe(
      map(response => {
        if(!response.isSuccess)
          throw new Error(response.errors![0].message);
        return response.data;
      }),
      catchError((err) => throwError(() => err))
    )
  }

  public getMusicianAvgByInstrument(query: AverageByInstrumentsQuery): Observable<number | null>{
    const searchUri = `${this.baseApiUrl}/averagebyinstrument`;
    return this.#httpclient
    .post<APIOperationResult<number>>(searchUri, query)
    .pipe(
      map(response => {
        if(!response.isSuccess)
          throw new Error(response.errors![0].message);
        return response.data;
      }),
      catchError((err) => throwError(() => err))
    );
  }

  public searchDomesticSeniorMusicians(age: number): Observable<number | null>{
    const searchUri = `${this.baseApiUrl}/searchdomesticseniormusicians`;

    return this.#httpclient.post<APIOperationResult<number>>(searchUri, { age })
    .pipe(
      map((response) => {
        if(!response.isSuccess)
          throw new Error(response.errors![0].message);
        return response.data;
      }),
      catchError((err) => throwError(() => err))
    );
  }


//#endregion

//#region Commands
public createMusician(command: CreateMusicianCommand): Observable<boolean>{
  const commandUri = `${this.baseApiUrl}/create`;

  return this.#httpclient.post<APIOperationResultBase>(commandUri, command)
  .pipe(
    map((response) => {
      if(!response.isSuccess)
        throw new Error(response.errors![0].message);
      return response.isSuccess;
    }),
    catchError((err) => throwError(() => err))
  )
}

public updateMusician(command: UpdateMusicianCommand): Observable<boolean>{
  const commandUri = `${this.baseApiUrl}/update`;

  return this.#httpclient.put<APIOperationResultBase>(commandUri, command)
  .pipe(
    map((response) => {
      if(!response.isSuccess)
        throw new Error(response.errors![0].message);
      return response.isSuccess;
    }),
    catchError((err) => throwError(() => err))
  )
}

public deleteMusician(id: number): Observable<boolean>{
  const commandUri = `${this.baseApiUrl}/${id}`;
  return this.#httpclient.delete<APIOperationResultBase>(commandUri)
  .pipe(
    map((response) => {
      if(!response.isSuccess){
        throw new Error(response.errors![0].message);
      }
      return response.isSuccess;
    }),
    catchError((err) => throwError(() => err))
  )
}

public deleteManyMusician(ids: number[]): Observable<boolean>{
  const commandUri = `${this.baseApiUrl}/deletemany`;

  return this.#httpclient.post<APIOperationResultBase>(commandUri, ids)
  .pipe(
    map((response) => response.isSuccess),
    catchError((err) => throwError(() => err))
  )
}

 public searchDomesticSeniorMusiciansAsync(query: SearchDomesticSeniorMusiciansQuery = {age: 30}): Observable<number>
  {
    const uri = `${this.baseApiUrl}/musician/domesticbyage`;

    return this.#httpclient.post<APIOperationResult<number>>(uri, query)
    .pipe(
      map(response => response.data!),
      catchError(err => {
        console.log(err);
        return throwError(() => err);
      })
    );
  }



  public getInternationalActivitiesByMusician(id: number): Observable<number>
  {
    const uri = `${this.baseApiUrl}/musician/internationalqty/${id}`;

    return this.#httpclient.get<APIOperationResult<number>>(uri)
    .pipe(
      map(response => response.data!),
      catchError(err => throwError(() => err))
    );
  }

  public getMusicianAverageByInstruments(instrumentIds: number[]): Observable<number>
  {
    const uri = `${this.baseApiUrl}/musician/averagebyinstrument`;

    return this.#httpclient.post<APIOperationResult<number>>(uri, {instrumentIds})
    .pipe(
      map(response => response.data!),
      catchError(err => throwError(() => err))
    )
  }

//#endregion
}

