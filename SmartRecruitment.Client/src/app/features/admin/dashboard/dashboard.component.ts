import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AdminService } from '../../../core/services/admin.service';
import { AdminDashboard } from '../../../core/models/admin.model';
import { AuthService } from '../../../core/services/auth.service';

interface Activity {
  icon: string;
  title: string;
  description: string;
  time: string;
  type: string;
}

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  isLoading = true;
  errorMessage = '';
  currentAdminName = 'Administrator';

  stats: AdminDashboard = {
    totalUsers: 0,
    totalJobSeekers: 0,
    totalEmployers: 0,
    activeUsers: 0,
    inactiveUsers: 0,
    totalJobs: 0
  };

  activities: Activity[] = [
    {
      icon: '👤',
      title: 'Platform Online',
      description: 'Platform control center initialized and ready.',
      time: 'Just now',
      type: 'system'
    },
    {
      icon: '🛡️',
      title: 'Security Verified',
      description: 'Role-based access token validated.',
      time: 'Just now',
      type: 'system'
    },
    {
      icon: '⚡',
      title: 'Matching Engine Active',
      description: 'AI skill recommendation algorithms active.',
      time: 'Live',
      type: 'match'
    }
  ];

  constructor(
    private adminService: AdminService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    const user = this.authService.getCurrentUser();
    if (user?.fullName) {
      this.currentAdminName = user.fullName;
    }
    this.loadDashboard();
  }

  loadDashboard(): void {
    this.isLoading = true;
    this.adminService.getDashboard().subscribe({
      next: (data) => {
        if (data) {
          this.stats = data;
        }
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Failed to load admin statistics.';
        this.isLoading = false;
      }
    });
  }

  getActiveUserPercentage(): number {
    if (!this.stats.totalUsers || this.stats.totalUsers === 0) {
      return 0;
    }
    return Math.round((this.stats.activeUsers / this.stats.totalUsers) * 100);
  }

  getInactiveUserPercentage(): number {
    if (!this.stats.totalUsers || this.stats.totalUsers === 0) {
      return 0;
    }
    return Math.round((this.stats.inactiveUsers / this.stats.totalUsers) * 100);
  }
}