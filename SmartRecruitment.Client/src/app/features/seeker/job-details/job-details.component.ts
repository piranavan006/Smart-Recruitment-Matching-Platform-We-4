import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

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
export class JobDetailsComponent {

  job = {
    title: 'Software Engineer',
    company: 'Tech Solutions',
    location: 'Colombo',
    salary: 'Rs. 100,000 - 150,000',
    type: 'Full Time',
    description:
      'Develop and maintain modern software applications using current technologies.',
    skills: [
      'C#',
      'ASP.NET Core',
      'Angular',
      'SQL Server',
      'Git'
    ],
    experience: '1 - 2 Years'
  };

}