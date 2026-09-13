import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  fullName: string;
  email: string;
  password: string;
  confirmPassword?: string;
  role: string;
}

export interface AuthResponse {
  userId: number;
  fullName: string;
  email: string;
  role: string;
  token: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'https://localhost:7000/api/Auth';

  constructor(private http: HttpClient) {}

  login(data: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(
      `${this.apiUrl}/login`,
      data
    );
  }

  register(data: RegisterRequest): Observable<any> {
  return this.http.post(
    `${this.apiUrl}/register`,
    data
  );
}

  forgotPassword(email: string): Observable<any> {
  return this.http.post(
    `${this.apiUrl}/forgot-password`,
    { email: email }
  );
}

  verifyOtp(email: string, otp: string): Observable<any> {

    return this.http.post(
      `${this.apiUrl}/verify-otp`,
      {
        email: email,
        otp: otp
      }
    );
  }

  resetPassword(data: {
    email: string;
    otp: string;
    newPassword: string;
    confirmPassword: string;
  }): Observable<any> {

    return this.http.post(
      `${this.apiUrl}/reset-password`,
      data
    );
  }

}
