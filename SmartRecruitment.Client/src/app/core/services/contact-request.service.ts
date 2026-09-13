import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ContactRequestResponse, CreateContactRequest, UpdateContactRequestStatus } from '../models/contact-request.model';

@Injectable({
  providedIn: 'root'
})
export class ContactRequestService {
  private apiUrl = `${environment.apiUrl}/ContactRequests`;

  constructor(private http: HttpClient) {}

  sendRequest(data: CreateContactRequest): Observable<ContactRequestResponse> {
    return this.http.post<ContactRequestResponse>(this.apiUrl, data);
  }

  getSent(): Observable<ContactRequestResponse[]> {
    return this.http.get<ContactRequestResponse[]>(`${this.apiUrl}/sent`);
  }

  getReceived(): Observable<ContactRequestResponse[]> {
    return this.http.get<ContactRequestResponse[]>(`${this.apiUrl}/received`);
  }

  respond(id: number, status: 'Accepted' | 'Declined'): Observable<ContactRequestResponse> {
    const payload: UpdateContactRequestStatus = { status };
    return this.http.put<ContactRequestResponse>(`${this.apiUrl}/${id}/status`, payload);
  }
}
