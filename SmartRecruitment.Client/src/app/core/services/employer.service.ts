import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CreateEmployerProfileRequest, EmployerProfile, UpdateEmployerProfileRequest } from '../models/employer.model';
import { MatchResult } from '../models/job.model';

@Injectable({
  providedIn: 'root'
})
export class EmployerService {
  private apiUrl = `${environment.apiUrl}/employers`;
  private matchingUrl = `${environment.apiUrl}/matching`;

  constructor(private http: HttpClient) {}

  getProfile(): Observable<EmployerProfile> {
    return this.http.get<EmployerProfile>(`${this.apiUrl}/profile`);
  }

  createProfile(data: CreateEmployerProfileRequest): Observable<EmployerProfile> {
    return this.http.post<EmployerProfile>(`${this.apiUrl}/profile`, data);
  }

  updateProfile(data: UpdateEmployerProfileRequest): Observable<EmployerProfile> {
    return this.http.put<EmployerProfile>(`${this.apiUrl}/profile`, data);
  }

  getMatchesForJob(jobId: number): Observable<MatchResult[]> {
    return this.http.get<MatchResult[]>(`${this.matchingUrl}/job/${jobId}`);
  }
}
