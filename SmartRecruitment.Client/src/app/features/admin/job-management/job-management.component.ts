import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AdminService } from '../../../core/services/admin.service';
import { JobService } from '../../../core/services/job.service';
import { Job } from '../../../core/models/job.model';

export interface AdminJob {
  id: number;
  title: string;
  company: string;
  location: string;
  type: string;
  applicants: number;
  postedDate: string;
  status: string;
  isEmployerActive?: boolean;
}

@Component({
  selector: 'app-job-management',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink
  ],
  templateUrl: './job-management.component.html',
  styleUrl: './job-management.component.css'
})
export class JobManagementComponent implements OnInit {
  searchTerm = '';
  selectedStatus = 'All';
  selectedType = 'All';

  jobs: AdminJob[] = [];
  isLoading = true;
  errorMessage = '';
  successMessage = '';
  actionError = '';
  deletingJobId: number | null = null;

  constructor(
    private adminService: AdminService,
    private jobService: JobService
  ) {}

  ngOnInit(): void {
    this.loadJobs();
  }

  loadJobs(): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.adminService.getJobs().subscribe({
      next: (res: Job[]) => {
        const rawItems = Array.isArray(res) ? res : [];
        this.jobs = rawItems.map((j: Job) => {
          const isEmployerActive = j.isEmployerActive !== false;
          const isClosed = j.isClosed || !isEmployerActive;
          return {
            id: j.id || 0,
            title: j.title || 'Untitled Vacancy',
            company: j.companyName || 'Company',
            location: j.location || 'Not Specified',
            type: j.employmentType || 'Full Time',
            applicants: j.applicantCount || 0,
            postedDate: j.createdAt ? new Date(j.createdAt).toLocaleDateString() : 'Recent',
            status: isClosed ? 'Closed' : 'Active',
            isEmployerActive: isEmployerActive
          };
        });
        this.isLoading = false;
      },
      error: (err) => {
        // Fallback to job search if needed
        this.jobService.searchJobs().subscribe({
          next: (res: Job[]) => {
            const rawItems = Array.isArray(res) ? res : [];
            this.jobs = rawItems.map((j: Job) => ({
              id: j.id || 0,
              title: j.title || 'Untitled Vacancy',
              company: j.companyName || 'Company',
              location: j.location || 'Not Specified',
              type: j.employmentType || 'Full Time',
              applicants: j.applicantCount || 0,
              postedDate: j.createdAt ? new Date(j.createdAt).toLocaleDateString() : 'Recent',
              status: j.isClosed ? 'Closed' : 'Active',
              isEmployerActive: true
            }));
            this.isLoading = false;
          },
          error: (fallbackErr) => {
            this.errorMessage = err.error?.message || fallbackErr.error?.message || 'Failed to load platform vacancies.';
            this.isLoading = false;
          }
        });
      }
    });
  }

  deleteJob(job: AdminJob): void {
    const confirmDelete = window.confirm(
      `Are you sure you want to permanently delete vacancy '${job.title}' (${job.company})?\n\nThis will remove the vacancy and all applicant records.`
    );

    if (!confirmDelete) {
      return;
    }

    this.deletingJobId = job.id;
    this.successMessage = '';
    this.actionError = '';

    this.adminService.deleteJob(job.id).subscribe({
      next: () => {
        this.jobs = this.jobs.filter(j => j.id !== job.id);
        this.successMessage = `Vacancy '${job.title}' was permanently deleted.`;
        this.deletingJobId = null;
        setTimeout(() => {
          this.successMessage = '';
        }, 4500);
      },
      error: (err) => {
        this.actionError = err.error?.message || 'Failed to delete vacancy. Please try again.';
        this.deletingJobId = null;
      }
    });
  }

  get filteredJobs(): AdminJob[] {
    return this.jobs.filter(job => {
      const search = this.searchTerm.toLowerCase().trim();
      const matchesSearch =
        job.title.toLowerCase().includes(search) ||
        job.company.toLowerCase().includes(search) ||
        job.location.toLowerCase().includes(search);

      const matchesStatus =
        this.selectedStatus === 'All' ||
        job.status.toLowerCase() === this.selectedStatus.toLowerCase();

      const matchesType =
        this.selectedType === 'All' ||
        job.type.toLowerCase().includes(this.selectedType.toLowerCase());

      return matchesSearch && matchesStatus && matchesType;
    });
  }

  getTotalJobs(): number {
    return this.jobs.length;
  }

  getActiveJobs(): number {
    return this.jobs.filter(job => job.status.toLowerCase() === 'active').length;
  }

  getClosedJobs(): number {
    return this.jobs.filter(job => job.status.toLowerCase() === 'closed').length;
  }

  getTotalApplicants(): number {
    return this.jobs.reduce((total, job) => total + (job.applicants || 0), 0);
  }

  getFullTimeJobs(): number {
    return this.jobs.filter(job => job.type.toLowerCase().includes('full')).length;
  }

  resetFilters(): void {
    this.searchTerm = '';
    this.selectedStatus = 'All';
    this.selectedType = 'All';
  }
}