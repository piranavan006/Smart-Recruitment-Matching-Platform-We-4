import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { JobService } from '../../../core/services/job.service';
import { JobResponse } from '../../../core/models/job.model';

@Component({
  selector: 'app-jobs',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink
  ],
  templateUrl: './jobs.component.html',
  styleUrl: './jobs.component.css'
})
export class JobsComponent implements OnInit {
  searchTitle = '';
  searchLocation = '';
  isLoading = false;
  jobs: JobResponse[] = [];

  constructor(private jobService: JobService) {}

  ngOnInit(): void {
    this.searchJobs();
  }

  searchJobs(): void {
    this.isLoading = true;
    this.jobService.searchJobs({
      keyword: this.searchTitle.trim() || undefined,
      location: this.searchLocation.trim() || undefined
    }).subscribe({
      next: (data) => {
        this.jobs = data;
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }

  formatSalary(min?: number, max?: number): string {
    if (!min && !max) return 'Negotiable';
    if (min && max) return `Rs. ${min.toLocaleString()} - ${max.toLocaleString()}`;
    if (min) return `From Rs. ${min.toLocaleString()}`;
    return `Up to Rs. ${max?.toLocaleString()}`;
  }
}