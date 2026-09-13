export interface AdminDashboard {
  totalUsers: number;
  totalJobSeekers: number;
  totalEmployers: number;
  activeUsers: number;
  inactiveUsers: number;
  totalJobs: number;
}

export interface AdminUser {
  userId: number;
  fullName: string;
  email: string;
  role: string;
  isActive: boolean;
}

export interface UpdateUserStatusRequest {
  isActive: boolean;
}

export interface AdminEmployer {
  id: number;
  userId: string;
  companyName: string;
  industry?: string;
  website?: string;
  description?: string;
  location?: string;
  isApproved: boolean;
  approvalStatus?: string;
}

export interface UpdateApprovalRequest {
  isApproved: boolean;
  status?: string;
  reason?: string;
}

