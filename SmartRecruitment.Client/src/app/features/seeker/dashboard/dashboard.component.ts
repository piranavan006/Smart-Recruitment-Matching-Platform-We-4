import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { JobService } from '../../../core/services/job.service';
import { ApplicationService } from '../../../core/services/application.service';
import { SeekerService } from '../../../core/services/seeker.service';
import { NotificationService } from '../../../core/services/notification.service';
import { AuthService } from '../../../core/services/auth.service';
import { MatchResult } from '../../../core/models/job.model';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  userName = '';
  availableJobs = 0;
  applicationsCount = 0;
  matchedJobsCount = 0;
  notificationsCount = 0;
  recommendedJobs: MatchResult[] = [];
  isLoading = true;

  constructor(
    private authService: AuthService,
    private jobService: JobService,
    private applicationService: ApplicationService,
    private seekerService: SeekerService,
    private notificationService: NotificationService
  ) {}

  ngOnInit(): void {
    const user = this.authService.getCurrentUser();
    if (user) {
      this.userName = user.fullName;
      this.loadDashboardData(user.userId);
    }
  }

  loadDashboardData(userId: number): void {
    this.isLoading = true;

    // Load available jobs
    this.jobService.searchJobs().subscribe({
      next: (jobs) => {
        this.availableJobs = jobs.filter(j => !j.isClosed).length;
      },
      error: () => {}
    });

    // Load applications count
    this.applicationService.getMyApplications().subscribe({
      next: (apps) => {
        this.applicationsCount = apps.length;
      },
      error: () => {}
    });

    // Load notifications count
    this.notificationService.getUserNotifications(userId).subscribe({
      next: (notifs) => {
        this.notificationsCount = notifs.filter(n => !n.isRead).length;
      },
      error: () => {}
    });

    // Load matched jobs
    this.seekerService.getMatchesForSeeker(userId).subscribe({
      next: (matches) => {
        this.matchedJobsCount = matches.filter(m => m.matchScore > 50).length;
        this.recommendedJobs = matches.slice(0, 3);
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }
}