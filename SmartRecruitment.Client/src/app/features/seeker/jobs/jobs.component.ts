import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-jobs',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink
  ],
  templateUrl: './jobs.component.html',
  styleUrl: './jobs.component.css'
})
export class JobsComponent {

  searchTitle = '';
  searchLocation = '';

  jobs = [
    {
      title: 'Software Engineer',
      company: 'Tech Solutions',
      location: 'Colombo',
      salary: 'Rs. 100,000 - 150,000',
      type: 'Full Time',
      description: 'Develop and maintain modern software applications using current technologies.'
    },
    {
      title: 'Frontend Developer',
      company: 'Digital Innovations',
      location: 'Jaffna',
      salary: 'Rs. 80,000 - 120,000',
      type: 'Full Time',
      description: 'Build responsive and user-friendly web applications for modern businesses.'
    },
    {
      title: 'Junior Web Developer',
      company: 'Creative Labs',
      location: 'Remote',
      salary: 'Rs. 60,000 - 90,000',
      type: 'Full Time',
      description: 'Work with the development team to create and improve web applications.'
    }
  ];

  allJobs = [...this.jobs];

  searchJobs(): void {

    const title = this.searchTitle.toLowerCase().trim();
    const location = this.searchLocation.toLowerCase().trim();

    this.jobs = this.allJobs.filter(job => {

      const matchesTitle =
        !title ||
        job.title.toLowerCase().includes(title);

      const matchesLocation =
        !location ||
        job.location.toLowerCase().includes(location);

      return matchesTitle && matchesLocation;
    });
  }
}