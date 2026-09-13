import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { JobService } from '../../../core/services/job.service';
import { ApplicationService } from '../../../core/services/application.service';
import { JobResponse } from '../../../core/models/job.model';

import { EmployerService } from '../../../core/services/employer.service';

export interface EmployerVacancyView extends JobResponse {
  applicantCount?: number;
}

@Component({
  selector: 'app-vacancies',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink
  ],
  templateUrl: './vacancies.component.html',
  styleUrl: './vacancies.component.css'
})
export class VacanciesComponent implements OnInit {
  vacancies: EmployerVacancyView[] = [];
  isLoading = true;
  isApproved = false;
  isLoadingProfile = true;
  successMessage = '';
  errorMessage = '';

  constructor(
    private jobService: JobService,
    private applicationService: ApplicationService,
    private employerService: EmployerService
  ) {}

  ngOnInit(): void {
    this.checkApproval();
    this.loadVacancies();
  }

  checkApproval(): void {
    this.isLoadingProfile = true;
    this.employerService.getProfile().subscribe({
      next: (profile) => {
        const isStatusApproved = profile?.approvalStatus
          ? profile.approvalStatus.toLowerCase() === 'approved'
          : !!profile?.isApproved;

        this.isApproved = !!profile?.isApproved && isStatusApproved;
        this.isLoadingProfile = false;
      },
      error: () => {
        this.isApproved = false;
        this.isLoadingProfile = false;
      }
    });
  }

  loadVacancies(): void {
    this.isLoading = true;
    this.jobService.getMyJobs().subscribe({
      next: (data) => {
        this.vacancies = data;
        this.isLoading = false;

        // Fetch applicant counts for each vacancy
        this.vacancies.forEach(v => {
          this.applicationService.getApplicationsByJob(v.id).subscribe({
            next: (apps) => {
              v.applicantCount = apps.length;
            },
            error: () => {
              v.applicantCount = 0;
            }
          });
        });
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }

  getActiveCount(): number {
    return this.vacancies.filter(v => !v.isClosed).length;
  }

  getClosedCount(): number {
    return this.vacancies.filter(v => v.isClosed).length;
  }

  getTotalApplicants(): number {
    return this.vacancies.reduce((total, v) => total + (v.applicantCount || 0), 0);
  }

  closeVacancy(id: number): void {
    if (!confirm('Are you sure you want to close this vacancy to new applications?')) return;

    this.jobService.closeJob(id).subscribe({
      next: () => {
        this.successMessage = 'Vacancy closed successfully.';
        this.loadVacancies();
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Failed to close vacancy.';
      }
    });
  }

  deleteVacancy(id: number): void {
    if (!confirm('Are you sure you want to delete this vacancy?')) return;

    this.jobService.deleteJob(id).subscribe({
      next: () => {
        this.successMessage = 'Vacancy deleted successfully.';
        this.loadVacancies();
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Failed to delete vacancy.';
      }
    });
  }
}