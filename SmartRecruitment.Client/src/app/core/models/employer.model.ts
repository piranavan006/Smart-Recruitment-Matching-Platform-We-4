export interface EmployerProfile {
  id?: number;
  employerProfileId?: number;
  userId?: string;
  companyName: string;
  industry?: string;
  location?: string;
  website?: string;
  companyDescription?: string;
  description?: string;
  phone?: string;
  companySize?: string;
  foundedYear?: number;
  isApproved?: boolean;
  approvalStatus?: string;
  createdAt?: string;
  updatedAt?: string;
}

export interface CreateEmployerProfileRequest {
  companyName: string;
  industry?: string;
  location?: string;
  website?: string;
  description?: string;
  phone?: string;
  companySize?: string;
  foundedYear?: number;
}

export interface UpdateEmployerProfileRequest extends CreateEmployerProfileRequest {}

