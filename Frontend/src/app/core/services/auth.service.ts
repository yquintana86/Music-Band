import { computed, effect, inject, Injectable, signal } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { catchError, map, Observable, tap, throwError } from 'rxjs';
import * as jose from 'jose';
import { environment } from '../../../environments/environment.development';
import { AuthStatusType, CredentialsResponse, JwtPayload, RegisterRequest, User } from '../../auth/interfaces';
import { APIOperationResult, APIOperationResultBase } from '../../shared/interfaces';



@Injectable({
  providedIn: 'root'
})
export class AuthService {

  //#region Private fields

  private readonly baseUrl: string = environment.API_BASE_URL;
  private httpClient = inject(HttpClient);

  private _currentUser = signal<User | null>(null);
  private _authStatusType = signal<AuthStatusType>(AuthStatusType.checking);
  private _accessToken = signal<string | null>(localStorage.getItem(environment.ACCESS_TOKEN_NAME));
  private _refreshToken = signal<string | null>(localStorage.getItem(environment.REFRESH_TOKEN_NAME));



  //#endregion

  constructor() {
    this.checkStatus();
  }

  //#region Public fields
  public currentUser = computed(() => this._currentUser());
  public authStatus = computed(() => this._authStatusType());
  public accessToken = computed(() => this._accessToken());
  public refreshToken = computed(() => this._refreshToken());

  //#endregion

  //#region Properties

  private accessTokenEffect = effect(() => {
    if (this._accessToken()) {
      localStorage.setItem(environment.ACCESS_TOKEN_NAME, this.accessToken() || '');
      return;
    }
    localStorage.removeItem(environment.ACCESS_TOKEN_NAME);
  });

  private refreshTokenEffect = effect(() => {
    if (this._refreshToken()) {
      localStorage.setItem(environment.REFRESH_TOKEN_NAME, this._refreshToken()!);
      return;
    }

    localStorage.removeItem(environment.REFRESH_TOKEN_NAME);
  });


  //#endregion

  //#region Public methods

  public login(email: string, password: string): Observable<boolean> {

    if (this._authStatusType() === AuthStatusType.authenticated) {
      return throwError(() => 'User is already logged in');
    }
    if (this._authStatusType() === AuthStatusType.checking) {
      return throwError(() => 'A logging request is been processed; please wait until the login status is confirmed');
    }

    const uri = `${this.baseUrl}/api/authentication/login`;
    const payload = { email, password };

    return this.httpClient.post<APIOperationResult<CredentialsResponse>>(uri, payload)
      .pipe(
        tap(({ data }) => {
          this.#signIn(data);
        }),
        map(() => true),
        catchError((err) => throwError(() => {
          this.#updateAuthStatus(AuthStatusType.notAuthenticated);
          return err;
        }))
      );
  }

  public register(registerRequest: RegisterRequest): Observable<boolean> {
    const registerUri = `${this.baseUrl}/api/authentication/register`;

    return this.httpClient.post<APIOperationResultBase>(registerUri, registerRequest)
      .pipe(
        map(response => response.isSuccess),
        catchError((err) => throwError(() => err))
      );
  }

  public checkStatus(): void {
    try {
      if (!this._accessToken() || !this._refreshToken()) {
        this.#updateAuthStatus(AuthStatusType.notAuthenticated);
        return;
      }

      let payload = jose.decodeJwt<JwtPayload>(this._accessToken()!);
      const { sub, exp }: JwtPayload = payload;
      if (this.#isExpiredByTokenExpiration(exp)) {
        this.renewCredentials(sub);
        payload = jose.decodeJwt<JwtPayload>(this._accessToken()!);
      }

      const user = this.#getUserFromPayload(payload);
      this._currentUser.set(user);
      this._authStatusType.set(AuthStatusType.authenticated);
    }
    catch {
      this.#updateAuthStatus(AuthStatusType.notAuthenticated);
    }
  }

  public logout(): Observable<boolean> {

    const url = `${this.baseUrl}/api/authentication/logout`;
    return this.httpClient.post<APIOperationResultBase>(url, JSON.stringify(this._refreshToken()),
      {
        headers: {
          'Content-Type': 'application/json'
        }
      })
      .pipe(
        tap(() => this.#updateAuthStatus(AuthStatusType.notAuthenticated)),
        map(() => true),
        catchError((err) => throwError(() => err))
      )
  }

  public renewCredentials(userId: number): Observable<void> {
    const refreshUri = `${this.baseUrl}/api/authentication/refreshtoken/${userId}`;

    return this.httpClient.post<APIOperationResult<CredentialsResponse>>(refreshUri, JSON.stringify(this._refreshToken()),
    {
      headers:{
        'Content-Type': 'application/json'
      }
    })
    .pipe(
        map(({ data: { accessToken, refreshToken } }) => {
          if (!accessToken || !refreshToken)
            throw Error('Invalid Tokens');

          this._accessToken.set(accessToken);
          this._refreshToken.set(refreshToken);
        }),
        catchError((err) => {
          this.#updateAuthStatus(AuthStatusType.notAuthenticated);
          return throwError(() => err);
        }
        ));
  }

  public _isExpiredToken(): boolean {
    if (!this._accessToken())
      return true;

    const { exp } = jose.decodeJwt(this._accessToken()!);
    return this.#isExpiredByTokenExpiration(exp);
  }

  //#endregion

  //#region Private Methods

  #clearTokens(): void {
    this._accessToken.set(null);
    this._refreshToken.set(null);
  }

  #signIn(credentialsResponse: CredentialsResponse): void {
    const { accessToken, refreshToken } = credentialsResponse;

    if (!refreshToken) {
      throw Error('Invalid tokens');
    }

    const payload = jose.decodeJwt<JwtPayload>(accessToken);
    const user = this.#getUserFromPayload(payload);
    this.#updateAuthStatus(AuthStatusType.authenticated, accessToken, refreshToken, user);
  }


  #updateAuthStatus(authStatusType: AuthStatusType, accessToken?: string, refreshToken?: string, user?: User): void {
    this._authStatusType.set(authStatusType);
    if (this._authStatusType() === AuthStatusType.authenticated && user) {
      this._currentUser.set(user);
      this._accessToken.set(accessToken!);
      this._refreshToken.set(refreshToken!);
      return;
    }

    this.#clearTokens();
    this._currentUser.set(null);
  }

  #isExpiredByTokenExpiration(exp?: number): boolean {
    if (!exp || exp! < 0)
      return true;

    const now = Math.floor(Date.now() / 1000);
    return now > exp;
  }

  #getUserFromPayload(payload: JwtPayload & jose.JWTPayload): User {
    try {
      return {
        sub: payload.sub,
        email: payload.email,
        given_name: payload.email,
        permission: payload.permission,
        aud: payload.aud,
        iss: payload.iss
      };
    }
    catch {
      throw Error('Invalid token');
    }
  }

  //#endregion

}


