import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CreateJobRequest, JobResponse, JobSearchParams, UpdateJobRequest } from '../models/job.model';

@Injectable({
  providedIn: 'root'
})
export class JobService {
  private apiUrl = `${environment.apiUrl}/jobs`;

  constructor(private http: HttpClient) {}

  searchJobs(params?: JobSearchParams): Observable<JobResponse[]> {
    let httpParams = new HttpParams();

    if (params) {
      if (params.keyword) httpParams = httpParams.set('keyword', params.keyword);
      if (params.location) httpParams = httpParams.set('location', params.location);
      if (params.education) httpParams = httpParams.set('education', params.education);
      if (params.minExperienceYears !== undefined && params.minExperienceYears !== null) {
        httpParams = httpParams.set('minExperienceYears', params.minExperienceYears.toString());
      }
      if (params.maxExperienceYears !== undefined && params.maxExperienceYears !== null) {
        httpParams = httpParams.set('maxExperienceYears', params.maxExperienceYears.toString());
      }
      if (params.salaryMin !== undefined && params.salaryMin !== null) {
        httpParams = httpParams.set('salaryMin', params.salaryMin.toString());
      }
      if (params.salaryMax !== undefined && params.salaryMax !== null) {
        httpParams = httpParams.set('salaryMax', params.salaryMax.toString());
      }
    }

    return this.http.get<JobResponse[]>(`${this.apiUrl}/search`, { params: httpParams });
  }

  getJobById(id: number): Observable<JobResponse> {
    return this.http.get<JobResponse>(`${this.apiUrl}/${id}`);
  }

  getMyJobs(): Observable<JobResponse[]> {
    return this.http.get<JobResponse[]>(`${this.apiUrl}/mine`);
  }

  createJob(data: CreateJobRequest): Observable<JobResponse> {
    return this.http.post<JobResponse>(this.apiUrl, data);
  }

  updateJob(id: number, data: UpdateJobRequest): Observable<JobResponse> {
    return this.http.put<JobResponse>(`${this.apiUrl}/${id}`, data);
  }

  deleteJob(id: number): Observable<{ message: string }> {
    return this.http.delete<{ message: string }>(`${this.apiUrl}/${id}`);
  }

  closeJob(id: number): Observable<{ message: string }> {
    return this.http.patch<{ message: string }>(`${this.apiUrl}/${id}/close`, {});
  }
}
