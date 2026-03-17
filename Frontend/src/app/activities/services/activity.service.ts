import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { catchError, map, Observable, throwError } from 'rxjs';
import { APIOperationResult, APIOperationResultBase, PagedResult } from '../../shared/interfaces';
import { ActivityFilterQuery, ActivityReponse, CreateActivityCommand, UpdateActivityCommand } from '../interfaces';

@Injectable({
  providedIn: 'root',
})
export class ActivityService {

private _httpClient = inject(HttpClient);
private baseUrl: string = `${environment.API_BASE_URL}/api/activity`;


public getAllActivities(): Observable<APIOperationResult<ActivityReponse>>{
  const uri = `${this.baseUrl}/list`;
  return this._httpClient.get<APIOperationResult<ActivityReponse>>(uri)
  .pipe(
    catchError(err => throwError(() => err))
  );
}

public getActivitiesByFilterQuery(filter: ActivityFilterQuery): Observable<PagedResult<ActivityReponse>>
{
  const searchUri = `${this.baseUrl}/search`;
  return this._httpClient.post<APIOperationResult<PagedResult<ActivityReponse>>>(searchUri, filter)
  .pipe(
    map(response => response.data!),
    catchError(err => throwError(() => err))
  )
}

public createActivity(activity: CreateActivityCommand): Observable<boolean>{
  const createUri = `${this.baseUrl}/create`;

  return this._httpClient.post<APIOperationResultBase>(createUri, activity)
  .pipe(
    map(response => response.isSuccess),
    catchError(err => throwError(() => err))
  )
}

public updateActivity(activity: UpdateActivityCommand): Observable<boolean>{
  const updateUri = `${this.baseUrl}/update`;

  return this._httpClient.put<APIOperationResultBase>(updateUri, activity)
  .pipe(
    map(response => response.isSuccess),
    catchError(err => throwError(() => err))
  )
}

public deleteActivity(id: number): Observable<boolean>{
  const deleteUri = `${this.baseUrl}/${id}`;

  return this._httpClient.delete<APIOperationResultBase>(deleteUri)
  .pipe(
    map(response => response.isSuccess),
    catchError(err => throwError(() => err))
  )
}

}
