import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-applications',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink
  ],
  templateUrl: './applications.component.html',
  styleUrl: './applications.component.css'
})
export class ApplicationsComponent {

  applications = [
    {
      jobTitle: 'Software Engineer',
      company: 'Tech Solutions',
      location: 'Colombo',
      appliedDate: '05 Sep 2026',
      status: 'Pending'
    },
    {
      jobTitle: 'Frontend Developer',
      company: 'Digital Innovations',
      location: 'Jaffna',
      appliedDate: '02 Sep 2026',
      status: 'Accepted'
    },
    {
      jobTitle: 'Junior Web Developer',
      company: 'Creative Labs',
      location: 'Remote',
      appliedDate: '28 Aug 2026',
      status: 'Rejected'
    }
  ];

  getCount(status: string): number {
    return this.applications.filter(
      application => application.status === status
    ).length;
  }

}