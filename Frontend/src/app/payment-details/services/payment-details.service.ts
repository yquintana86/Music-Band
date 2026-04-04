import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { catchError, map, Observable, throwError } from 'rxjs';

import { environment } from '../../../environments/environment.development';
import { CreatePaymentDetailCommand, SearchPaymentDetailsByFilterQuery, UpdatePaymentDetailCommand } from '../interfaces';
import { APIOperationResult, APIOperationResultBase, PagedResult } from '../../shared/interfaces';
import { PaymentDetailResponse } from '../interfaces/payment-detail-response.interface';

@Injectable({
  providedIn: 'root'
})
export class PaymentDetailService {
  private _httpClient = inject(HttpClient);
  private baseUrl: string = `${environment.API_BASE_URL}/api/paymentdetail`;


  public searchPaymentDetailsByQueryFilter(filter: SearchPaymentDetailsByFilterQuery): Observable<PagedResult<PaymentDetailResponse>>
  {
    const uri = `${this.baseUrl}/search`;

    return this._httpClient.post<APIOperationResult<PagedResult<PaymentDetailResponse>>>(uri, filter)
    .pipe(
      map(response => response.data),
      catchError((err) => throwError(() => err))
    )
  }

  public createPaymentDetail(command: CreatePaymentDetailCommand): Observable<boolean>
  {
    const uri = `${this.baseUrl}/create`;
    return this._httpClient.post<APIOperationResultBase>(uri, command)
    .pipe(
      map(response => response.isSuccess),
      catchError((err) => throwError(() => err))
    );
  }

  public updatePaymentDetail(command: UpdatePaymentDetailCommand): Observable<boolean>
  {
    const uri = `${this.baseUrl}/update`;

    return this._httpClient.put<APIOperationResultBase>(uri, command)
    .pipe(
      map(response => response.isSuccess),
      catchError((err) => throwError(() => err))
    )
  }

  public deletePaymentDetail(id: number): Observable<boolean>
  {
    const uri = `${this.baseUrl}/${id}`;

    return this._httpClient.delete<APIOperationResultBase>(uri)
    .pipe(
      map(response => response.isSuccess),
      catchError((err) => throwError(() => err))
    );
  }

  public deleteManyPaymentDetails(ids: number[]): Observable<boolean>
  {
    const uri = `${this.baseUrl}/deletemany`;

    return this._httpClient.post<APIOperationResultBase>(uri, ids)
    .pipe(
      map(response => response.isSuccess),
      catchError((err) => throwError(() => err))
    )
  }
}
