import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

interface AdminUser {
  id: number;
  name: string;
  email: string;
  role: string;
  status: string;
  joinedDate: string;
}

@Component({
  selector: 'app-user-management',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink
  ],
  templateUrl: './user-management.component.html',
  styleUrl: './user-management.component.css'
})
export class UserManagementComponent {

  searchTerm = '';
  selectedRole = 'All';
  selectedStatus = 'All';

  users: AdminUser[] = [
    {
      id: 1,
      name: 'Arun Kumar',
      email: 'arun@example.com',
      role: 'Job Seeker',
      status: 'Active',
      joinedDate: '05 Sep 2026'
    },
    {
      id: 2,
      name: 'Sathya Priya',
      email: 'sathya@example.com',
      role: 'Job Seeker',
      status: 'Active',
      joinedDate: '04 Sep 2026'
    },
    {
      id: 3,
      name: 'Tech Solutions',
      email: 'hr@techsolutions.com',
      role: 'Employer',
      status: 'Active',
      joinedDate: '02 Sep 2026'
    },
    {
      id: 4,
      name: 'Ravi Kumar',
      email: 'ravi@example.com',
      role: 'Job Seeker',
      status: 'Inactive',
      joinedDate: '01 Sep 2026'
    },
    {
      id: 5,
      name: 'Digital Innovations',
      email: 'admin@digitalinnovations.com',
      role: 'Employer',
      status: 'Active',
      joinedDate: '30 Aug 2026'
    },
    {
      id: 6,
      name: 'Nimal Perera',
      email: 'nimal@example.com',
      role: 'Job Seeker',
      status: 'Inactive',
      joinedDate: '28 Aug 2026'
    }
  ];


  // Filter users
  get filteredUsers(): AdminUser[] {

    return this.users.filter(user => {

      const matchesSearch =
        user.name
          .toLowerCase()
          .includes(this.searchTerm.toLowerCase()) ||

        user.email
          .toLowerCase()
          .includes(this.searchTerm.toLowerCase());


      const matchesRole =
        this.selectedRole === 'All' ||
        user.role === this.selectedRole;


      const matchesStatus =
        this.selectedStatus === 'All' ||
        user.status === this.selectedStatus;


      return (
        matchesSearch &&
        matchesRole &&
        matchesStatus
      );

    });

  }


  // Total users
  getTotalUsers(): number {

    return this.users.length;

  }


  // Active users
  getActiveUsers(): number {

    return this.users.filter(
      user => user.status === 'Active'
    ).length;

  }


  // Inactive users
  getInactiveUsers(): number {

    return this.users.filter(
      user => user.status === 'Inactive'
    ).length;

  }


  // Job seekers
  getJobSeekers(): number {

    return this.users.filter(
      user => user.role === 'Job Seeker'
    ).length;

  }


  // Employers
  getEmployers(): number {

    return this.users.filter(
      user => user.role === 'Employer'
    ).length;

  }


  // Activate / deactivate user
  toggleUserStatus(user: AdminUser): void {

    if (user.status === 'Active') {

      user.status = 'Inactive';

    } else {

      user.status = 'Active';

    }

  }


  // Reset filters
  resetFilters(): void {

    this.searchTerm = '';
    this.selectedRole = 'All';
    this.selectedStatus = 'All';

  }

}