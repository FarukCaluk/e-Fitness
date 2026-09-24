import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthResponse, AuthenticatedUser, LoginRequest, RegisterRequest } from '../models/auth.model';
import { TokenStorageService } from './token-storage.service';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly currentUserSubject: BehaviorSubject<AuthenticatedUser | null>;
  readonly currentUser$;

  constructor(private readonly http: HttpClient, private readonly tokenStorage: TokenStorageService) {
    this.currentUserSubject = new BehaviorSubject<AuthenticatedUser | null>(this.tokenStorage.getUser());
    this.currentUser$ = this.currentUserSubject.asObservable();
  }

  register(request: RegisterRequest): Observable<{ id: number }> {
    return this.http.post<{ id: number }>(`${environment.apiUrl}/auth/register`, request);
  }

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${environment.apiUrl}/auth/login`, request, { withCredentials: true })
      .pipe(tap((response) => this.setSession(response)));
  }

  refreshToken(): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${environment.apiUrl}/auth/refresh`, {}, { withCredentials: true })
      .pipe(tap((response) => this.setSession(response)));
  }

  logout(): Observable<void> {
    return this.http.post<void>(`${environment.apiUrl}/auth/logout`, {}, { withCredentials: true }).pipe(
      tap(() => this.clearSession())
    );
  }

  clearSession(): void {
    this.tokenStorage.clear();
    this.currentUserSubject.next(null);
  }

  isAuthenticated(): boolean {
    return !!this.tokenStorage.getAccessToken();
  }

  getCurrentUser(): AuthenticatedUser | null {
    return this.currentUserSubject.value;
  }

  private setSession(response: AuthResponse): void {
    this.tokenStorage.setSession(response.accessToken, response.user);
    this.currentUserSubject.next(response.user);
  }
}
