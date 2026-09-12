import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

interface AdminJob {
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
export class JobManagementComponent {

  searchTerm = '';
  selectedStatus = 'All';
  selectedType = 'All';

  jobs: AdminJob[] = [
    {
      id: 1,
      title: 'Software Engineer',
      company: 'Tech Solutions',
      location: 'Colombo',
      type: 'Full Time',
      applicants: 12,
      postedDate: '08 Sep 2026',
      status: 'Active'
    },
    {
      id: 2,
      title: 'Frontend Developer',
      company: 'Digital Innovations',
      location: 'Jaffna',
      type: 'Full Time',
      applicants: 8,
      postedDate: '07 Sep 2026',
      status: 'Active'
    },
    {
      id: 3,
      title: 'Junior Web Developer',
      company: 'Creative Labs',
      location: 'Remote',
      type: 'Full Time',
      applicants: 5,
      postedDate: '05 Sep 2026',
      status: 'Closed'
    },
    {
      id: 4,
      title: 'Backend Developer',
      company: 'Tech Lanka',
      location: 'Kandy',
      type: 'Full Time',
      applicants: 9,
      postedDate: '03 Sep 2026',
      status: 'Active'
    },
    {
      id: 5,
      title: 'UI/UX Designer',
      company: 'Creative Labs',
      location: 'Colombo',
      type: 'Part Time',
      applicants: 6,
      postedDate: '01 Sep 2026',
      status: 'Closed'
    },
    {
      id: 6,
      title: 'Angular Developer',
      company: 'Digital Innovations',
      location: 'Remote',
      type: 'Full Time',
      applicants: 11,
      postedDate: '30 Aug 2026',
      status: 'Active'
    }
  ];


  // Filter jobs
  get filteredJobs(): AdminJob[] {

    return this.jobs.filter(job => {

      const search =
        this.searchTerm.toLowerCase().trim();

      const matchesSearch =
        job.title.toLowerCase().includes(search) ||
        job.company.toLowerCase().includes(search) ||
        job.location.toLowerCase().includes(search);

      const matchesStatus =
        this.selectedStatus === 'All' ||
        job.status === this.selectedStatus;

      const matchesType =
        this.selectedType === 'All' ||
        job.type === this.selectedType;

      return (
        matchesSearch &&
        matchesStatus &&
        matchesType
      );

    });

  }


  // Total jobs
  getTotalJobs(): number {

    return this.jobs.length;

  }


  // Active jobs
  getActiveJobs(): number {

    return this.jobs.filter(
      job => job.status === 'Active'
    ).length;

  }


  // Closed jobs
  getClosedJobs(): number {

    return this.jobs.filter(
      job => job.status === 'Closed'
    ).length;

  }


  // Total applicants
  getTotalApplicants(): number {

    return this.jobs.reduce(
      (total, job) => total + job.applicants,
      0
    );

  }


  // Full-time jobs
  getFullTimeJobs(): number {

    return this.jobs.filter(
      job => job.type === 'Full Time'
    ).length;

  }


  // Close a job
  closeJob(job: AdminJob): void {

    job.status = 'Closed';

  }


  // Re-open a closed job
  activateJob(job: AdminJob): void {

    job.status = 'Active';

  }


  // Reset filters
  resetFilters(): void {

    this.searchTerm = '';
    this.selectedStatus = 'All';
    this.selectedType = 'All';

  }

}