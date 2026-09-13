import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { JobService } from '../../../core/services/job.service';
import { ApplicationService } from '../../../core/services/application.service';
import { AuthService } from '../../../core/services/auth.service';
import { JobResponse } from '../../../core/models/job.model';

@Component({
  selector: 'app-job-details',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink
  ],
  templateUrl: './job-details.component.html',
  styleUrl: './job-details.component.css'
})
export class JobDetailsComponent implements OnInit {
  jobId!: number;
  job: JobResponse | null = null;
  isLoading = true;
  isApplying = false;
  hasApplied = false;
  successMessage = '';
  errorMessage = '';

  userRole = '';
  isAdmin = false;
  isEmployer = false;
  isJobSeeker = true;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private jobService: JobService,
    private applicationService: ApplicationService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    const user = this.authService.getCurrentUser();
    if (user) {
      this.userRole = (user.role || '').toLowerCase();
      this.isAdmin = this.userRole.includes('admin');
      this.isEmployer = this.userRole.includes('employer');
      this.isJobSeeker = !this.isAdmin && !this.isEmployer;
    }

    const idParam = this.route.snapshot.paramMap.get('id') || this.route.snapshot.queryParamMap.get('id');
    if (idParam) {
      this.jobId = Number(idParam);
      this.loadJobDetails(this.jobId);
      if (this.isJobSeeker) {
        this.checkApplicationStatus(this.jobId);
      }
    } else {
      this.errorMessage = 'No job ID specified.';
      this.isLoading = false;
    }
  }

  get backRoute(): string {
    if (this.isAdmin) return '/admin/job-management';
    if (this.isEmployer) return '/employer/vacancies';
    return '/seeker/jobs';
  }

  get backLabel(): string {
    if (this.isAdmin) return '← Back to Job Directory';
    if (this.isEmployer) return '← Back to My Vacancies';
    return '← Back to Jobs';
  }

  loadJobDetails(id: number): void {
    this.isLoading = true;
    this.jobService.getJobById(id).subscribe({
      next: (data) => {
        this.job = data;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Job not found or has been removed.';
        this.isLoading = false;
      }
    });
  }

  checkApplicationStatus(jobId: number): void {
    if (!this.authService.isLoggedIn()) return;

    this.applicationService.getMyApplications().subscribe({
      next: (apps) => {
        this.hasApplied = apps.some(a => a.jobId === jobId);
      },
      error: () => {}
    });
  }

  applyNow(): void {
    if (!this.authService.isLoggedIn()) {
      this.router.navigate(['/login'], { queryParams: { returnUrl: `/seeker/job-details/${this.jobId}` } });
      return;
    }

    if (this.hasApplied) return;

    this.isApplying = true;
    this.successMessage = '';
    this.errorMessage = '';

    this.applicationService.apply(this.jobId).subscribe({
      next: () => {
        this.isApplying = false;
        this.hasApplied = true;
        this.successMessage = 'Your application has been submitted successfully!';
      },
      error: (err) => {
        this.isApplying = false;
        if (err.status === 409) {
          this.hasApplied = true;
          this.errorMessage = 'You have already applied for this vacancy.';
        } else {
          this.errorMessage = err.error?.message || 'Failed to submit application. Please try again.';
        }
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