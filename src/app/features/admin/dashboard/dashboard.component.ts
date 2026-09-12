import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

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
export class DashboardComponent {

  // Platform statistics
  stats = {
    totalUsers: 248,
    activeUsers: 221,
    inactiveUsers: 27,
    totalJobs: 86,
    totalApplications: 634,
    matchedCandidates: 412
  };


  // Recruitment performance
  recruitmentStats = {
    successfulMatches: 68,
    pendingApplications: 124,
    acceptedApplications: 87,
    rejectedApplications: 43
  };


  // Recent system activities
  activities: Activity[] = [

    {
      icon: '👤',
      title: 'New User Registered',
      description:
        'A new job seeker account was created.',
      time: '10 minutes ago',
      type: 'user'
    },

    {
      icon: '💼',
      title: 'New Vacancy Posted',
      description:
        'A new Software Engineer vacancy was published.',
      time: '35 minutes ago',
      type: 'job'
    },

    {
      icon: '⭐',
      title: 'High Match Detected',
      description:
        'A candidate achieved a 94% job matching score.',
      time: '1 hour ago',
      type: 'match'
    },

    {
      icon: '📄',
      title: 'New Application',
      description:
        'A candidate submitted a new job application.',
      time: '2 hours ago',
      type: 'application'
    },

    {
      icon: '🔔',
      title: 'System Notification',
      description:
        'Platform activity has been updated.',
      time: '3 hours ago',
      type: 'system'
    }

  ];


  // Quick statistics
  getActiveUserPercentage(): number {

    if (this.stats.totalUsers === 0) {
      return 0;
    }

    return Math.round(
      (this.stats.activeUsers / this.stats.totalUsers) * 100
    );

  }


  getInactiveUserPercentage(): number {

    if (this.stats.totalUsers === 0) {
      return 0;
    }

    return Math.round(
      (this.stats.inactiveUsers / this.stats.totalUsers) * 100
    );

  }


  getApplicationSuccessRate(): number {

    if (this.stats.totalApplications === 0) {
      return 0;
    }

    return Math.round(
      (
        this.recruitmentStats.acceptedApplications /
        this.stats.totalApplications
      ) * 100
    );

  }

}