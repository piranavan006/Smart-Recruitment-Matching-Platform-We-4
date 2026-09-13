export interface JobSkill {
  skillId?: number;
  skillName: string;
  weight: number;
}

export interface JobResponse {
  id: number;
  employerProfileId: number;
  companyName?: string;
  title: string;
  description: string;
  location?: string;
  education?: string;
  employmentType?: string;
  minExperienceYears: number;
  maxExperienceYears: number;
  salaryMin?: number;
  salaryMax?: number;
  applicationDeadline: string;
  isClosed: boolean;
  applicantCount?: number;
  isEmployerActive?: boolean;
  employerApprovalStatus?: string;
  requiredSkills: JobSkill[];
  createdAt: string;
  updatedAt: string;
}

export type Job = JobResponse;


export interface CreateJobRequest {
  title: string;
  description: string;
  location?: string;
  education?: string;
  minExperienceYears: number;
  maxExperienceYears: number;
  salaryMin?: number;
  salaryMax?: number;
  applicationDeadline: string;
  requiredSkills: JobSkill[];
}

export interface UpdateJobRequest extends CreateJobRequest {}

export interface JobSearchParams {
  keyword?: string;
  location?: string;
  education?: string;
  minExperienceYears?: number;
  maxExperienceYears?: number;
  salaryMin?: number;
  salaryMax?: number;
}

export interface MatchResult {
  jobId: number;
  jobSeekerId: number;
  jobTitle?: string;
  candidateName?: string;
  matchScore: number;
  missingSkills: string[];
  skillsMatched: boolean;
  experienceMatched: boolean;
  educationMatched: boolean;
  locationMatched: boolean;
}
