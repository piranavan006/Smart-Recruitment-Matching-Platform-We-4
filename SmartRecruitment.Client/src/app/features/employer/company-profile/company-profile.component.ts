import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { EmployerService } from '../../../core/services/employer.service';
import { AuthService } from '../../../core/services/auth.service';
import { EmployerProfile } from '../../../core/models/employer.model';

@Component({
  selector: 'app-company-profile',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink
  ],
  templateUrl: './company-profile.component.html',
  styleUrl: './company-profile.component.css'
})
export class CompanyProfileComponent implements OnInit {
  profile: EmployerProfile = {
    companyName: '',
    industry: '',
    location: '',
    website: '',
    description: ''
  };

  businessEmail = '';
  isLoading = true;
  isSaving = false;
  successMessage = '';
  errorMessage = '';

  constructor(
    private employerService: EmployerService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    const user = this.authService.getCurrentUser();
    if (user) {
      this.businessEmail = user.email;
      this.profile.companyName = user.fullName;
    }
    this.loadProfile();
  }

  loadProfile(): void {
    this.isLoading = true;
    this.employerService.getProfile().subscribe({
      next: (data) => {
        if (data) {
          this.profile = {
            ...data,
            description: data.description || data.companyDescription || ''
          };
        }
        this.isLoading = false;
      },
      error: () => {
        // If not created yet, allow creating one
        this.isLoading = false;
      }
    });
  }

  saveProfile(): void {
    if (!this.profile.companyName?.trim()) {
      this.errorMessage = 'Company name is required.';
      return;
    }

    this.isSaving = true;
    this.successMessage = '';
    this.errorMessage = '';

    const request$ = this.profile.employerProfileId
      ? this.employerService.updateProfile(this.profile)
      : this.employerService.createProfile(this.profile);

    request$.subscribe({
      next: (saved) => {
        this.profile = {
          ...saved,
          description: saved.description || saved.companyDescription || ''
        };
        this.isSaving = false;
        this.successMessage = 'Company profile updated successfully!';
      },
      error: (err) => {
        this.isSaving = false;
        this.errorMessage = err.error?.message || 'Failed to save company profile.';
      }
    });
  }
}