import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

interface ContactRequest {
  initials: string;
  name: string;
  jobTitle: string;
  email: string;
  location: string;
  time: string;
  message: string;
  status: string;
}

@Component({
  selector: 'app-contact-requests',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink
  ],
  templateUrl: './contact-requests.component.html',
  styleUrl: './contact-requests.component.css'
})
export class ContactRequestsComponent {

  requests: ContactRequest[] = [
    {
      initials: 'AK',
      name: 'Arun Kumar',
      jobTitle: 'Software Engineer',
      email: 'arun@example.com',
      location: 'Colombo',
      time: '2 hours ago',
      message: 'I would like to connect regarding the Software Engineer opportunity.',
      status: 'Pending'
    },
    {
      initials: 'SP',
      name: 'Sathya Priya',
      jobTitle: 'Frontend Developer',
      email: 'sathya@example.com',
      location: 'Jaffna',
      time: '1 day ago',
      message: 'I am interested in discussing the Frontend Developer position.',
      status: 'Accepted'
    },
    {
      initials: 'RK',
      name: 'Ravi Kumar',
      jobTitle: 'Software Engineer',
      email: 'ravi@example.com',
      location: 'Kandy',
      time: '2 days ago',
      message: 'I would like to learn more about the available position.',
      status: 'Pending'
    }
  ];

  getCount(status: string): number {
    return this.requests.filter(
      request => request.status === status
    ).length;
  }

  updateStatus(
    request: ContactRequest,
    status: string
  ): void {
    request.status = status;
  }

}