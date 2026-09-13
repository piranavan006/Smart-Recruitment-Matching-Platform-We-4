import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApplicationResponse, UpdateApplicationStatusRequest } from '../models/application.model';

@Injectable({
  providedIn: 'root'
})
export class ApplicationService {
  private apiUrl = `${environment.apiUrl}/Applications`;

  constructor(private http: HttpClient) {}

  apply(jobId: number): Observable<ApplicationResponse> {
    return this.http.post<ApplicationResponse>(this.apiUrl, { jobId });
  }

  getMyApplications(): Observable<ApplicationResponse[]> {
    return this.http.get<ApplicationResponse[]>(`${this.apiUrl}/my`);
  }

  getApplicationsByJob(jobId: number): Observable<ApplicationResponse[]> {
    return this.http.get<ApplicationResponse[]>(`${this.apiUrl}/job/${jobId}`);
  }

  getById(id: number): Observable<ApplicationResponse> {
    return this.http.get<ApplicationResponse>(`${this.apiUrl}/${id}`);
  }

  updateStatus(id: number, status: string): Observable<ApplicationResponse> {
    const payload: UpdateApplicationStatusRequest = { status };
    return this.http.put<ApplicationResponse>(`${this.apiUrl}/${id}/status`, payload);
  }
}
