import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { SeekerService } from '../../../core/services/seeker.service';
import { AuthService } from '../../../core/services/auth.service';
import { CvResponse, JobSeekerProfile } from '../../../core/models/seeker.model';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {
  fullName = '';
  email = '';

  profile: JobSeekerProfile = {
    summary: '',
    skills: '',
    experience: '',
    education: '',
    location: ''
  };

  currentCv: CvResponse | null = null;
  newSkillInput = '';
  skillsList: string[] = [];

  isLoading = true;
  isSaving = false;
  isUploadingCv = false;
  successMessage = '';
  errorMessage = '';

  constructor(
    private seekerService: SeekerService,
    private authService: AuthService
  ) {}

  get profileCompleteness(): number {
    let score = 20;
    if (this.profile.location && this.profile.location.trim().length > 0) score += 15;
    if (this.profile.summary && this.profile.summary.trim().length > 0) score += 20;
    if (this.skillsList && this.skillsList.length > 0) score += 20;
    if (this.profile.education && this.profile.education.trim().length > 0) score += 10;
    if (this.currentCv) score += 15;
    return Math.min(score, 100);
  }

  ngOnInit(): void {
    const user = this.authService.getCurrentUser();
    if (user) {
      this.fullName = user.fullName;
      this.email = user.email;
    }
    this.loadProfile();
    this.loadCv();
  }

  loadProfile(): void {
    this.isLoading = true;
    this.seekerService.getMyProfile().subscribe({
      next: (data) => {
        if (data) {
          this.profile = data;
          this.parseSkills(data.skills);
        }
        this.isLoading = false;
      },
      error: () => {
        // If profile doesn't exist yet, we allow creating one
        this.isLoading = false;
      }
    });
  }

  loadCv(): void {
    this.seekerService.getMyCv().subscribe({
      next: (cv) => {
        this.currentCv = cv;
      },
      error: () => {
        this.currentCv = null;
      }
    });
  }

  parseSkills(skillsString?: string): void {
    if (!skillsString) {
      this.skillsList = [];
      return;
    }
    this.skillsList = skillsString
      .split(',')
      .map(s => s.trim())
      .filter(s => s.length > 0);
  }

  addSkill(): void {
    const skill = this.newSkillInput.trim();
    if (skill && !this.skillsList.includes(skill)) {
      this.skillsList.push(skill);
      this.newSkillInput = '';
      this.profile.skills = this.skillsList.join(', ');
    }
  }

  removeSkill(index: number): void {
    this.skillsList.splice(index, 1);
    this.profile.skills = this.skillsList.join(', ');
  }

  saveProfile(): void {
    this.isSaving = true;
    this.successMessage = '';
    this.errorMessage = '';

    this.profile.skills = this.skillsList.join(', ');

    const request$ = this.profile.jobSeekerProfileId
      ? this.seekerService.updateProfile(this.profile)
      : this.seekerService.createProfile(this.profile);

    request$.subscribe({
      next: (saved) => {
        this.profile = saved;
        this.isSaving = false;
        this.successMessage = 'Profile updated successfully!';
      },
      error: (err) => {
        this.isSaving = false;
        this.errorMessage = err.error?.message || 'Failed to save profile. Please try again.';
      }
    });
  }

  onFileSelected(event: Event): void {
    const target = event.target as HTMLInputElement;
    if (target.files && target.files.length > 0) {
      const file = target.files[0];
      this.uploadCvFile(file);
    }
  }

  uploadCvFile(file: File): void {
    this.isUploadingCv = true;
    this.successMessage = '';
    this.errorMessage = '';

    this.seekerService.uploadCv(file).subscribe({
      next: (cv) => {
        this.currentCv = cv;
        this.isUploadingCv = false;
        this.successMessage = 'CV uploaded successfully!';
      },
      error: (err) => {
        this.isUploadingCv = false;
        this.errorMessage = err.error?.message || 'Failed to upload CV. PDF, DOC and DOCX up to 5MB allowed.';
      }
    });
  }

  deleteCv(): void {
    if (!confirm('Are you sure you want to delete your uploaded CV?')) return;

    this.seekerService.deleteCv().subscribe({
      next: () => {
        this.currentCv = null;
        this.successMessage = 'CV removed successfully.';
      },
      error: () => {
        this.errorMessage = 'Failed to delete CV.';
      }
    });
  }
}