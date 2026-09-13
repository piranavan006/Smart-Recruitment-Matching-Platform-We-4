import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ContactRequestService } from '../../../core/services/contact-request.service';
import { ContactRequestResponse } from '../../../core/models/contact-request.model';

@Component({
  selector: 'app-contact-requests',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink
  ],
  templateUrl: './contact-requests.component.html',
  styleUrl: './contact-requests.component.css'
})
export class ContactRequestsComponent implements OnInit {
  requests: ContactRequestResponse[] = [];
  isLoading = true;
  errorMessage = '';

  constructor(private contactRequestService: ContactRequestService) {}

  ngOnInit(): void {
    this.loadRequests();
  }

  loadRequests(): void {
    this.isLoading = true;
    this.contactRequestService.getSent().subscribe({
      next: (data) => {
        this.requests = data || [];
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Failed to load contact requests.';
        this.isLoading = false;
      }
    });
  }

  getCount(status: string): number {
    return this.requests.filter(
      r => r.status?.toLowerCase() === status.toLowerCase()
    ).length;
  }
}