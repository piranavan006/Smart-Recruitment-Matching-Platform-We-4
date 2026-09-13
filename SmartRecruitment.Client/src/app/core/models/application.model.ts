export interface ApplicationResponse {
  applicationId: number;
  jobId: number;
  jobSeekerId: number;
  status: string;
  matchScore?: number;
  appliedAt: string;
  updatedAt: string;
  // UI helper fields
  jobTitle?: string;
  companyName?: string;
  location?: string;
  candidateName?: string;
}

export interface CreateApplicationRequest {
  jobId: number;
}

export interface UpdateApplicationStatusRequest {
  status: 'Pending' | 'Under Review' | 'Accepted' | 'Rejected' | string;
}
