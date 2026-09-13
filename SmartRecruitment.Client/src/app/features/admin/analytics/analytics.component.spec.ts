import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

interface MonthlyData {
  month: string;
  users: number;
  jobs: number;
  applications: number;
}

interface ActivityData {
  label: string;
  value: number;
  percentage: number;
}

@Component({
  selector: 'app-admin-analytics',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink
  ],
  templateUrl: './analytics.component.html',
  styleUrl: './analytics.component.css'
})
export class AnalyticsComponent {

  /* =====================================================
     OVERVIEW
     ===================================================== */

  overview = {
    totalUsers: 248,
    activeUsers: 221,
    totalJobs: 86,
    totalApplications: 634,
    matchedCandidates: 412,
    successfulMatches: 68
  };


  /* =====================================================
     ANALYTICS PERIOD
     ===================================================== */

  selectedPeriod = '6 Months';

  periods: string[] = [
    '1 Month',
    '3 Months',
    '6 Months',
    '1 Year'
  ];


  /* =====================================================
     PERIOD DATA
     ===================================================== */

  periodData: Record<string, MonthlyData[]> = {

    '1 Month': [
      {
        month: 'Week 1',
        users: 8,
        jobs: 3,
        applications: 24
      },
      {
        month: 'Week 2',
        users: 11,
        jobs: 4,
        applications: 31
      },
      {
        month: 'Week 3',
        users: 9,
        jobs: 3,
        applications: 27
      },
      {
        month: 'Week 4',
        users: 12,
        jobs: 4,
        applications: 35
      }
    ],

    '3 Months': [
      {
        month: 'Jul',
        users: 47,
        jobs: 17,
        applications: 112
      },
      {
        month: 'Aug',
        users: 52,
        jobs: 18,
        applications: 126
      },
      {
        month: 'Sep',
        users: 40,
        jobs: 12,
        applications: 138
      }
    ],

    '6 Months': [
      {
        month: 'Apr',
        users: 32,
        jobs: 11,
        applications: 74
      },
      {
        month: 'May',
        users: 41,
        jobs: 15,
        applications: 96
      },
      {
        month: 'Jun',
        users: 36,
        jobs: 13,
        applications: 88
      },
      {
        month: 'Jul',
        users: 47,
        jobs: 17,
        applications: 112
      },
      {
        month: 'Aug',
        users: 52,
        jobs: 18,
        applications: 126
      },
      {
        month: 'Sep',
        users: 40,
        jobs: 12,
        applications: 138
      }
    ],

    '1 Year': [
      {
        month: 'Jan',
        users: 25,
        jobs: 9,
        applications: 61
      },
      {
        month: 'Feb',
        users: 29,
        jobs: 10,
        applications: 68
      },
      {
        month: 'Mar',
        users: 31,
        jobs: 11,
        applications: 72
      },
      {
        month: 'Apr',
        users: 32,
        jobs: 11,
        applications: 74
      },
      {
        month: 'May',
        users: 41,
        jobs: 15,
        applications: 96
      },
      {
        month: 'Jun',
        users: 36,
        jobs: 13,
        applications: 88
      },
      {
        month: 'Jul',
        users: 47,
        jobs: 17,
        applications: 112
      },
      {
        month: 'Aug',
        users: 52,
        jobs: 18,
        applications: 126
      },
      {
        month: 'Sep',
        users: 40,
        jobs: 12,
        applications: 138
      }
    ]

  };


  /* =====================================================
     APPLICATION STATUS
     ===================================================== */

  applicationStatus: ActivityData[] = [

    {
      label: 'Accepted',
      value: 87,
      percentage: 14
    },

    {
      label: 'Pending',
      value: 124,
      percentage: 20
    },

    {
      label: 'Rejected',
      value: 43,
      percentage: 7
    },

    {
      label: 'Other',
      value: 380,
      percentage: 59
    }

  ];


  /* =====================================================
     USER DISTRIBUTION
     ===================================================== */

  userDistribution: ActivityData[] = [

    {
      label: 'Job Seekers',
      value: 174,
      percentage: 70
    },

    {
      label: 'Employers',
      value: 68,
      percentage: 27
    },

    {
      label: 'Administrators',
      value: 6,
      percentage: 3
    }

  ];


  /* =====================================================
     CURRENT CHART DATA
     ===================================================== */

  get monthlyData(): MonthlyData[] {

    return this.periodData[this.selectedPeriod] || [];

  }


  /* =====================================================
     CHANGE PERIOD
     ===================================================== */

  changePeriod(period: string): void {

    if (this.periodData[period]) {
      this.selectedPeriod = period;
    }

  }


  /* =====================================================
     ACTIVE USER PERCENTAGE
     ===================================================== */

  getActiveUserPercentage(): number {

    if (this.overview.totalUsers === 0) {
      return 0;
    }

    return Math.round(
      (this.overview.activeUsers /
        this.overview.totalUsers) * 100
    );

  }


  /* =====================================================
     MATCH RATE
     ===================================================== */

  getMatchRate(): number {

    if (this.overview.totalApplications === 0) {
      return 0;
    }

    return Math.round(
      (this.overview.matchedCandidates /
        this.overview.totalApplications) * 100
    );

  }


  /* =====================================================
     SUCCESS RATE
     ===================================================== */

  getSuccessRate(): number {

    if (this.overview.totalApplications === 0) {
      return 0;
    }

    return Math.round(
      (this.overview.successfulMatches /
        this.overview.totalApplications) * 100
    );

  }


  /* =====================================================
     MAX APPLICATIONS
     ===================================================== */

  getMaxApplications(): number {

    if (this.monthlyData.length === 0) {
      return 0;
    }

    return Math.max(
      ...this.monthlyData.map(
        data => data.applications
      )
    );

  }


  /* =====================================================
     APPLICATION BAR HEIGHT
     ===================================================== */

  getApplicationBarHeight(value: number): number {

    const max = this.getMaxApplications();

    if (max === 0) {
      return 0;
    }

    return Math.round(
      (value / max) * 100
    );

  }

}