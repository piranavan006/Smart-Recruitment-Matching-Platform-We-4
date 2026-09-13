import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
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

  constructor(private jobService: JobService) {}

  ngOnInit(): void {
    this.loadJobs();
  }

  loadJobs(): void {
    this.isLoading = true;
    this.jobService.searchJobs().subscribe({
      next: (res: Job[]) => {
        const rawItems = Array.isArray(res) ? res : [];
        this.jobs = rawItems.map((j: Job) => ({
          id: j.id || 0,
          title: j.title || 'Untitled Vacancy',
          company: j.companyName || 'Company',
          location: j.location || 'Not Specified',
          type: j.employmentType || 'Full Time',
          applicants: 0,
          postedDate: j.createdAt ? new Date(j.createdAt).toLocaleDateString() : 'Recent',
          status: j.isClosed ? 'Closed' : 'Active'
        }));
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Failed to load platform vacancies.';
        this.isLoading = false;
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