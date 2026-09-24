import { Injectable } from '@angular/core';
import { AuthenticatedUser } from '../models/auth.model';

@Injectable({ providedIn: 'root' })
export class TokenStorageService {
  private accessToken: string | null = null;
  private currentUser: AuthenticatedUser | null = null;

  setSession(accessToken: string, user: AuthenticatedUser): void {
    this.accessToken = accessToken;
    this.currentUser = user;
    sessionStorage.setItem('efitness_user', JSON.stringify(user));
  }

  getAccessToken(): string | null {
    return this.accessToken;
  }

  setAccessToken(accessToken: string): void {
    this.accessToken = accessToken;
  }

  getUser(): AuthenticatedUser | null {
    if (this.currentUser) {
      return this.currentUser;
    }

    const stored = sessionStorage.getItem('efitness_user');
    if (!stored) {
      return null;
    }

    this.currentUser = JSON.parse(stored) as AuthenticatedUser;
    return this.currentUser;
  }

  clear(): void {
    this.accessToken = null;
    this.currentUser = null;
    sessionStorage.removeItem('efitness_user');
  }
}
