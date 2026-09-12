import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

interface Applicant {
  rank: number;
  initials: string;
  name: string;
  jobTitle: string;
  location: string;
  experience: number;
  appliedDate: string;
  skills: string[];
  matchScore: number;
  status: string;
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
export class ApplicantsComponent {

  selectedJob = '';

  applicants: Applicant[] = [
    {
      rank: 1,
      initials: 'AK',
      name: 'Arun Kumar',
      jobTitle: 'Software Engineer',
      location: 'Colombo',
      experience: 3,
      appliedDate: '08 Sep 2026',
      skills: ['C#', 'ASP.NET Core', 'SQL'],
      matchScore: 94,
      status: 'Pending'
    },
    {
      rank: 2,
      initials: 'SP',
      name: 'Sathya Priya',
      jobTitle: 'Frontend Developer',
      location: 'Jaffna',
      experience: 2,
      appliedDate: '07 Sep 2026',
      skills: ['Angular', 'TypeScript', 'CSS'],
      matchScore: 89,
      status: 'Accepted'
    },
    {
      rank: 3,
      initials: 'RK',
      name: 'Ravi Kumar',
      jobTitle: 'Software Engineer',
      location: 'Kandy',
      experience: 4,
      appliedDate: '06 Sep 2026',
      skills: ['C#', 'SQL', 'JavaScript'],
      matchScore: 84,
      status: 'Pending'
    },
    {
      rank: 4,
      initials: 'NM',
      name: 'Nimal Perera',
      jobTitle: 'Frontend Developer',
      location: 'Colombo',
      experience: 1,
      appliedDate: '05 Sep 2026',
      skills: ['HTML', 'CSS', 'JavaScript'],
      matchScore: 78,
      status: 'Rejected'
    }
  ];

  getTopMatch(): number {
    if (this.applicants.length === 0) {
      return 0;
    }

    return Math.max(
      ...this.applicants.map(applicant => applicant.matchScore)
    );
  }

  getCount(status: string): number {
    return this.applicants.filter(
      applicant => applicant.status === status
    ).length;
  }

}