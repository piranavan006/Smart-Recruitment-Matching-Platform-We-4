import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AdminService } from '../../../core/services/admin.service';
import { AdminEmployer, AdminUser } from '../../../core/models/admin.model';

export interface DisplayUser {
  userId: number;
  name: string;
  email: string;
  role: string;
  status: 'Active' | 'Inactive';
  isActive: boolean;
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
export class UserManagementComponent implements OnInit {
  activeTab: 'users' | 'employers' = 'users';

  searchTerm = '';
  selectedRole = 'All';
  selectedStatus = 'All';

  users: DisplayUser[] = [];
  employers: AdminEmployer[] = [];

  isLoading = true;
  isLoadingEmployers = false;
  errorMessage = '';
  successMessage = '';

  constructor(private adminService: AdminService) {}

  ngOnInit(): void {
    this.loadUsers();
    this.loadEmployers();
  }

  loadUsers(): void {
    this.isLoading = true;
    this.adminService.getUsers().subscribe({
      next: (data) => {
        this.users = (data || []).map(u => ({
          userId: u.userId,
          name: u.fullName || u.email,
          email: u.email,
          role: this.normalizeRole(u.role),
          status: u.isActive ? 'Active' : 'Inactive',
          isActive: u.isActive,
          joinedDate: 'Registered'
        }));
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Failed to load user directory.';
        this.isLoading = false;
      }
    });
  }

  loadEmployers(): void {
    this.isLoadingEmployers = true;
    this.adminService.getEmployers().subscribe({
      next: (data) => {
        this.employers = data || [];
        this.isLoadingEmployers = false;
      },
      error: () => {
        this.isLoadingEmployers = false;
      }
    });
  }

  approveEmployer(emp: AdminEmployer): void {
    this.successMessage = '';
    this.errorMessage = '';

    this.adminService.updateEmployerApproval(emp.id, true, 'Approved').subscribe({
      next: () => {
        emp.isApproved = true;
        emp.approvalStatus = 'Approved';
        this.successMessage = `Company '${emp.companyName}' has been approved successfully!`;
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Failed to approve company profile.';
      }
    });
  }

  rejectEmployer(emp: AdminEmployer): void {
    const reason = window.prompt(`Enter rejection reason for '${emp.companyName}' (optional):`, 'Profile details do not meet platform verification criteria.');
    if (reason === null) {
      return; // Admin clicked cancel
    }

    this.successMessage = '';
    this.errorMessage = '';

    this.adminService.updateEmployerApproval(emp.id, false, 'Rejected', reason).subscribe({
      next: () => {
        emp.isApproved = false;
        emp.approvalStatus = 'Rejected';
        this.successMessage = `Company '${emp.companyName}' profile has been rejected.`;
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Failed to reject company profile.';
      }
    });
  }

  revokeEmployerApproval(emp: AdminEmployer): void {
    this.successMessage = '';
    this.errorMessage = '';

    this.adminService.updateEmployerApproval(emp.id, false, 'Pending').subscribe({
      next: () => {
        emp.isApproved = false;
        emp.approvalStatus = 'Pending';
        this.successMessage = `Company '${emp.companyName}' approval has been revoked.`;
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Failed to revoke company approval.';
      }
    });
  }

  getPendingEmployerApprovals(): number {
    return this.employers.filter(e => !e.isApproved && e.approvalStatus !== 'Rejected').length;
  }

  get filteredEmployers(): AdminEmployer[] {
    const term = this.searchTerm.toLowerCase().trim();
    if (!term) return this.employers;
    return this.employers.filter(e =>
      e.companyName.toLowerCase().includes(term) ||
      (e.industry && e.industry.toLowerCase().includes(term)) ||
      (e.location && e.location.toLowerCase().includes(term))
    );
  }

  private normalizeRole(role: string): string {
    if (!role) return 'Job Seeker';
    const lower = role.toLowerCase();
    if (lower.includes('admin')) return 'Administrator';
    if (lower.includes('employer')) return 'Employer';
    return 'Job Seeker';
  }

  get filteredUsers(): DisplayUser[] {
    return this.users.filter(user => {
      const matchesSearch =
        user.name.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        user.email.toLowerCase().includes(this.searchTerm.toLowerCase());

      const matchesRole =
        this.selectedRole === 'All' ||
        user.role === this.selectedRole;

      const matchesStatus =
        this.selectedStatus === 'All' ||
        user.status === this.selectedStatus;

      return matchesSearch && matchesRole && matchesStatus;
    });
  }

  getTotalUsers(): number {
    return this.users.length;
  }

  getActiveUsers(): number {
    return this.users.filter(user => user.isActive).length;
  }

  getInactiveUsers(): number {
    return this.users.filter(user => !user.isActive).length;
  }

  getJobSeekers(): number {
    return this.users.filter(user => user.role === 'Job Seeker').length;
  }

  getEmployers(): number {
    return this.users.filter(user => user.role === 'Employer').length;
  }

  toggleUserStatus(user: DisplayUser): void {
    const targetStatus = !user.isActive;
    this.successMessage = '';
    this.errorMessage = '';

    this.adminService.updateUserStatus(user.userId, targetStatus).subscribe({
      next: () => {
        user.isActive = targetStatus;
        user.status = targetStatus ? 'Active' : 'Inactive';
        this.successMessage = `User ${user.name} is now ${user.status}.`;
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Failed to update user status.';
      }
    });
  }

  deleteUser(user: DisplayUser): void {
    if (user.email.toLowerCase() === 'admin@smartrecruitment.com') {
      alert('The primary platform administrator cannot be deleted.');
      return;
    }

    const confirmDelete = window.confirm(
      `Are you sure you want to permanently delete user '${user.name}' (${user.email})? This action cannot be undone.`
    );
    if (!confirmDelete) {
      return;
    }

    this.successMessage = '';
    this.errorMessage = '';

    this.adminService.deleteUser(user.userId).subscribe({
      next: () => {
        this.users = this.users.filter(u => u.userId !== user.userId);
        this.successMessage = `User '${user.name}' has been permanently deleted.`;
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Failed to delete user account.';
      }
    });
  }

  resetFilters(): void {
    this.searchTerm = '';
    this.selectedRole = 'All';
    this.selectedStatus = 'All';
  }
}