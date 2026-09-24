import { UserRole } from './user-role.enum';

export interface AuthenticatedUser {
  id: number;
  email: string;
  firstName: string;
  lastName: string;
  role: UserRole;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface AuthResponse {
  accessToken: string;
  accessTokenExpiresAt: string;
  user: AuthenticatedUser;
}
