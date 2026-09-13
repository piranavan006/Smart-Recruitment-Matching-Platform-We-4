import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ApplicationService } from '../../../core/services/application.service';
import { JobService } from '../../../core/services/job.service';
import { ApplicationResponse } from '../../../core/models/application.model';

@Component({
  selector: 'app-applications',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink
  ],
  templateUrl: './applications.component.html',
  styleUrl: './applications.component.css'
})
export class ApplicationsComponent implements OnInit {
  applications: ApplicationResponse[] = [];
  isLoading = true;

  constructor(
    private applicationService: ApplicationService,
    private jobService: JobService
  ) {}

  ngOnInit(): void {
    this.loadApplications();
  }

  loadApplications(): void {
    this.isLoading = true;
    this.applicationService.getMyApplications().subscribe({
      next: (data) => {
        this.applications = data;
        this.isLoading = false;

        // Enrich with job info
        this.applications.forEach(app => {
          this.jobService.getJobById(app.jobId).subscribe({
            next: (job) => {
              app.jobTitle = job.title;
              app.companyName = job.companyName || 'Verified Employer';
              app.location = job.location || 'Location Not Specified';
            },
            error: () => {
              app.jobTitle = `Job Position #${app.jobId}`;
              app.companyName = 'Employer';
            }
          });
        });
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }

  getCount(status: string): number {
    return this.applications.filter(
      application => application.status.toLowerCase() === status.toLowerCase()
    ).length;
  }
}