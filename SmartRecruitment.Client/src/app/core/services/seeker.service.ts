import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CreateProfileRequest, CvResponse, JobSeekerProfile, UpdateProfileRequest } from '../models/seeker.model';
import { MatchResult } from '../models/job.model';

@Injectable({
  providedIn: 'root'
})
export class SeekerService {
  private seekerUrl = `${environment.apiUrl}/jobseekers`;
  private cvUrl = `${environment.apiUrl}/cv`;
  private matchingUrl = `${environment.apiUrl}/matching`;

  constructor(private http: HttpClient) {}

  getMyProfile(): Observable<JobSeekerProfile> {
    return this.http.get<JobSeekerProfile>(`${this.seekerUrl}/profile`);
  }

  createProfile(data: CreateProfileRequest): Observable<JobSeekerProfile> {
    return this.http.post<JobSeekerProfile>(`${this.seekerUrl}/profile`, data);
  }

  updateProfile(data: UpdateProfileRequest): Observable<JobSeekerProfile> {
    return this.http.put<JobSeekerProfile>(`${this.seekerUrl}/profile`, data);
  }

  uploadCv(file: File): Observable<CvResponse> {
    const formData = new FormData();
    formData.append('file', file, file.name);
    return this.http.post<CvResponse>(`${this.cvUrl}/upload`, formData);
  }

  getMyCv(): Observable<CvResponse> {
    return this.http.get<CvResponse>(this.cvUrl);
  }

  deleteCv(): Observable<{ message: string }> {
    return this.http.delete<{ message: string }>(this.cvUrl);
  }

  getMatchesForSeeker(seekerId: number): Observable<MatchResult[]> {
    return this.http.get<MatchResult[]>(`${this.matchingUrl}/jobseeker/${seekerId}`);
  }
}
