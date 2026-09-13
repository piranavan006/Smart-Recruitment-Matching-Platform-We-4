import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { JobService } from '../../../core/services/job.service';
import { ApplicationService } from '../../../core/services/application.service';
import { ContactRequestService } from '../../../core/services/contact-request.service';
import { AuthService } from '../../../core/services/auth.service';
import { EmployerService } from '../../../core/services/employer.service';
import { JobResponse } from '../../../core/models/job.model';
import { EmployerProfile } from '../../../core/models/employer.model';

@Component({
  selector: 'app-employer-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  companyName = '';
  fullName = '';
  totalVacancies = 0;
  activeVacancies = 0;
  totalApplicants = 0;
  pendingRequests = 0;
  myJobs: JobResponse[] = [];
  employerProfile: EmployerProfile | null = null;
  hasProfile = false;
  isApproved = false;
  approvalStatus = 'Pending';
  isLoading = true;

  constructor(
    private authService: AuthService,
    private jobService: JobService,
    private applicationService: ApplicationService,
    private contactRequestService: ContactRequestService,
    private employerService: EmployerService
  ) {}

  ngOnInit(): void {
    const user = this.authService.getCurrentUser();
    if (user) {
      this.fullName = user.fullName;
      this.companyName = user.fullName;
    }
    this.loadProfileAndDashboard();
  }

  loadProfileAndDashboard(): void {
    this.employerService.getProfile().subscribe({
      next: (profile) => {
        if (profile) {
          this.employerProfile = profile;
          this.hasProfile = true;
          this.isApproved = !!profile.isApproved;
          this.approvalStatus = profile.approvalStatus || (profile.isApproved ? 'Approved' : 'Pending');
          if (profile.companyName) {
            this.companyName = profile.companyName;
          }
        }
      },
      error: () => {}
    });

    this.loadDashboard();
  }

  loadDashboard(): void {
    this.isLoading = true;

    this.jobService.getMyJobs().subscribe({
      next: (jobs) => {
        this.myJobs = jobs;
        this.totalVacancies = jobs.length;
        this.activeVacancies = jobs.filter(j => !j.isClosed).length;

        // Fetch total applicants count
        let count = 0;
        let completed = 0;

        if (jobs.length === 0) {
          this.totalApplicants = 0;
          this.isLoading = false;
          return;
        }

        jobs.forEach(j => {
          this.applicationService.getApplicationsByJob(j.id).subscribe({
            next: (apps) => {
              count += apps.length;
              completed++;
              if (completed === jobs.length) {
                this.totalApplicants = count;
                this.isLoading = false;
              }
            },
            error: () => {
              completed++;
              if (completed === jobs.length) {
                this.totalApplicants = count;
                this.isLoading = false;
              }
            }
          });
        });
      },
      error: () => {
        this.isLoading = false;
      }
    });

    this.contactRequestService.getSent().subscribe({
      next: (requests) => {
        this.pendingRequests = requests.filter(r => r.status === 'Pending').length;
      },
      error: () => {}
    });
  }
}