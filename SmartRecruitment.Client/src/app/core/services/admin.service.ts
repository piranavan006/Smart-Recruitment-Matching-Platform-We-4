import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AdminDashboard, AdminEmployer, AdminUser, UpdateApprovalRequest, UpdateUserStatusRequest } from '../models/admin.model';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private apiUrl = `${environment.apiUrl}/Admin`;

  constructor(private http: HttpClient) {}

  getDashboard(): Observable<AdminDashboard> {
    return this.http.get<AdminDashboard>(`${this.apiUrl}/dashboard`);
  }

  getUsers(): Observable<AdminUser[]> {
    return this.http.get<AdminUser[]>(`${this.apiUrl}/users`);
  }

  updateUserStatus(userId: number, isActive: boolean): Observable<{ message: string; userId: number; isActive: boolean }> {
    const payload: UpdateUserStatusRequest = { isActive };
    return this.http.put<{ message: string; userId: number; isActive: boolean }>(`${this.apiUrl}/users/${userId}/status`, payload);
  }

  getEmployers(): Observable<AdminEmployer[]> {
    return this.http.get<AdminEmployer[]>(`${this.apiUrl}/employers`);
  }

  updateEmployerApproval(employerProfileId: number, isApproved: boolean, status?: string, reason?: string): Observable<{ message: string; employer: AdminEmployer }> {
    const payload: UpdateApprovalRequest = { isApproved, status, reason };
    return this.http.put<{ message: string; employer: AdminEmployer }>(`${this.apiUrl}/employers/${employerProfileId}/approval`, payload);
  }

  deleteUser(userId: number): Observable<{ message: string; userId?: number }> {
    return this.http.delete<{ message: string; userId?: number }>(`${this.apiUrl}/users/${userId}`);
  }
}

