import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink
  ],
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.css'
})
export class ResetPasswordComponent implements OnInit {

  email = '';
  otp = '';
  newPassword = '';
  confirmPassword = '';

  isLoading = false;
  errorMessage = '';
  successMessage = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {

    // Get email and OTP saved from Forgot Password / Verify OTP
    const savedEmail = localStorage.getItem('resetEmail');
    const savedOtp = localStorage.getItem('resetOtp');

    if (savedEmail) {
      this.email = savedEmail;
    }
    if (savedOtp) {
      this.otp = savedOtp;
    }
  }

  onSubmit(form: NgForm): void {

    if (form.invalid) {
      return;
    }

    this.errorMessage = '';
    this.successMessage = '';

    if (this.newPassword !== this.confirmPassword) {
      this.errorMessage = 'Passwords do not match.';
      return;
    }

    this.isLoading = true;

    const resetRequest = {
      email: this.email,
      otp: this.otp,
      newPassword: this.newPassword,
      confirmPassword: this.confirmPassword
    };

    this.authService.resetPassword(resetRequest).subscribe({

      next: (response) => {

        console.log('Reset password successful:', response);

        this.isLoading = false;

        this.successMessage =
          'Password reset successfully. You can now login.';

        // Remove saved reset email and OTP
        localStorage.removeItem('resetEmail');
        localStorage.removeItem('resetOtp');

        // Go to Login
        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 1500);
      },

      error: (error) => {

        console.error('Reset password error:', error);

        this.isLoading = false;

        if (error.status === 400) {

          this.errorMessage =
            error.error?.message ||
            'Invalid reset password request.';

        } else if (error.status === 404) {

          this.errorMessage =
            'Email address not found.';

        } else {

          this.errorMessage =
            'Unable to reset password. Please try again.';
        }
      }
    });
  }
}