export interface JobSeekerProfile {
  jobSeekerProfileId?: number;
  userId?: number;
  summary: string;
  skills: string;
  experience: string;
  education: string;
  location: string;
  cvFileName?: string;
  cvFilePath?: string;
  cvUploadedAt?: string;
}

export interface CreateProfileRequest {
  summary?: string;
  skills?: string;
  experience?: string;
  education?: string;
  location?: string;
}

export interface UpdateProfileRequest extends CreateProfileRequest {}

export interface CvResponse {
  jobSeekerProfileId: number;
  cvFileName: string;
  cvFilePath: string;
  cvUploadedAt: string;
}
