import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ApplicationService } from '../../../core/services/application.service';
import { JobService } from '../../../core/services/job.service';
import { EmployerService } from '../../../core/services/employer.service';
import { ContactRequestService } from '../../../core/services/contact-request.service';
import { JobResponse, MatchResult } from '../../../core/models/job.model';
import { ApplicationResponse } from '../../../core/models/application.model';

export interface EnrichedApplicant extends ApplicationResponse {
  rank?: number;
  matchScoreVal?: number;
  matchedSkills?: string[];
  missingSkills?: string[];
}

@Component({
  selector: 'app-applicants',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink
  ],
  templateUrl: './applicants.component.html',
  styleUrl: './applicants.component.css'
})
export class ApplicantsComponent implements OnInit {
  vacancies: JobResponse[] = [];
  selectedJobId: number | null = null;
  applicants: EnrichedApplicant[] = [];
  isLoading = true;
  successMessage = '';
  errorMessage = '';

  // Contact request modal / inline state
  contactingApplicant: EnrichedApplicant | null = null;
  contactMessage = '';
  isSendingMessage = false;

  constructor(
    private route: ActivatedRoute,
    private jobService: JobService,
    private applicationService: ApplicationService,
    private employerService: EmployerService,
    private contactRequestService: ContactRequestService
  ) {}

  ngOnInit(): void {
    this.loadEmployerVacancies();
  }

  loadEmployerVacancies(): void {
    this.isLoading = true;
    this.jobService.getMyJobs().subscribe({
      next: (jobs) => {
        this.vacancies = jobs;

        const queryJobId = this.route.snapshot.queryParamMap.get('jobId');
        if (queryJobId && jobs.some(j => j.id === Number(queryJobId))) {
          this.selectedJobId = Number(queryJobId);
        } else if (jobs.length > 0) {
          this.selectedJobId = jobs[0].id;
        }

        if (this.selectedJobId) {
          this.loadApplicantsForJob(this.selectedJobId);
        } else {
          this.isLoading = false;
        }
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }

  onJobChange(): void {
    if (this.selectedJobId) {
      this.loadApplicantsForJob(this.selectedJobId);
    } else {
      this.applicants = [];
    }
  }

  loadApplicantsForJob(jobId: number): void {
    this.isLoading = true;
    this.applicationService.getApplicationsByJob(jobId).subscribe({
      next: (apps) => {
        // Fetch AI matching results to enrich applications
        this.employerService.getMatchesForJob(jobId).subscribe({
          next: (matches) => {
            this.enrichApplicants(apps, matches);
            this.isLoading = false;
          },
          error: () => {
            this.enrichApplicants(apps, []);
            this.isLoading = false;
          }
        });
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }

  enrichApplicants(apps: ApplicationResponse[], matches: MatchResult[]): void {
    const matchMap = new Map<number, MatchResult>();
    matches.forEach(m => matchMap.set(m.jobSeekerId, m));

    this.applicants = apps.map((app, index) => {
      const match = matchMap.get(app.jobSeekerId);
      return {
        ...app,
        rank: index + 1,
        matchScoreVal: match ? Math.round(match.matchScore) : (app.matchScore ? Number(app.matchScore) : 75),
        candidateName: match?.candidateName || `Candidate #${app.jobSeekerId}`,
        missingSkills: match?.missingSkills || []
      };
    }).sort((a, b) => (b.matchScoreVal || 0) - (a.matchScoreVal || 0));

    // Re-rank after sorting
    this.applicants.forEach((a, i) => a.rank = i + 1);
  }

  updateStatus(app: EnrichedApplicant, newStatus: string): void {
    this.applicationService.updateStatus(app.applicationId, newStatus).subscribe({
      next: (updated) => {
        app.status = updated.status;
        this.successMessage = `Applicant status updated to ${newStatus}.`;
        setTimeout(() => this.successMessage = '', 3000);
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Failed to update status.';
        setTimeout(() => this.errorMessage = '', 3000);
      }
    });
  }

  openContactModal(app: EnrichedApplicant): void {
    this.contactingApplicant = app;
    this.contactMessage = `Hello ${app.candidateName}, we reviewed your application and would like to schedule an interview.`;
  }

  closeContactModal(): void {
    this.contactingApplicant = null;
    this.contactMessage = '';
  }

  sendContactMessage(): void {
    if (!this.contactingApplicant || !this.contactMessage.trim()) return;

    this.isSendingMessage = true;
    this.contactRequestService.sendRequest({
      receiverId: this.contactingApplicant.jobSeekerId,
      message: this.contactMessage.trim()
    }).subscribe({
      next: () => {
        this.isSendingMessage = false;
        this.successMessage = 'Contact request sent successfully!';
        this.closeContactModal();
        setTimeout(() => this.successMessage = '', 3000);
      },
      error: (err) => {
        this.isSendingMessage = false;
        this.errorMessage = err.error?.message || 'Failed to send contact request.';
        setTimeout(() => this.errorMessage = '', 3000);
      }
    });
  }

  getTopMatch(): number {
    if (this.applicants.length === 0) return 0;
    return Math.max(...this.applicants.map(a => a.matchScoreVal || 0));
  }

  getCount(status: string): number {
    return this.applicants.filter(a => a.status.toLowerCase() === status.toLowerCase()).length;
  }
}