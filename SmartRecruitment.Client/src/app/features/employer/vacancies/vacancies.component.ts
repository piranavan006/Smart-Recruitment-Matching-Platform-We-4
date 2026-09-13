import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

interface Vacancy {
  title: string;
  company: string;
  location: string;
  salary: string;
  type: string;
  applicants: number;
  skills: string[];
  status: string;
}

@Component({
  selector: 'app-vacancies',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink
  ],
  templateUrl: './vacancies.component.html',
  styleUrl: './vacancies.component.css'
})
export class VacanciesComponent {

  vacancies: Vacancy[] = [
    {
      title: 'Software Engineer',
      company: 'Tech Solutions',
      location: 'Colombo',
      salary: 'Rs. 100,000 - 150,000',
      type: 'Full Time',
      applicants: 12,
      skills: ['C#', 'ASP.NET Core', 'SQL'],
      status: 'Active'
    },
    {
      title: 'Frontend Developer',
      company: 'Digital Innovations',
      location: 'Jaffna',
      salary: 'Rs. 80,000 - 120,000',
      type: 'Full Time',
      applicants: 8,
      skills: ['Angular', 'TypeScript', 'CSS'],
      status: 'Active'
    },
    {
      title: 'Junior Web Developer',
      company: 'Creative Labs',
      location: 'Remote',
      salary: 'Rs. 60,000 - 90,000',
      type: 'Full Time',
      applicants: 5,
      skills: ['HTML', 'CSS', 'JavaScript'],
      status: 'Closed'
    }
  ];

  getCount(status: string): number {
    return this.vacancies.filter(
      vacancy => vacancy.status === status
    ).length;
  }

  getTotalApplicants(): number {
    return this.vacancies.reduce(
      (total, vacancy) => total + vacancy.applicants,
      0
    );
  }

}