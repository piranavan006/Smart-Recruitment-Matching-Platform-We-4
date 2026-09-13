import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-verify-otp',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink
  ],
  templateUrl: './verify-otp.component.html',
  styleUrl: './verify-otp.component.css'
})
export class VerifyOtpComponent implements OnInit {

  email = '';
  otp = '';

  isLoading = false;
  errorMessage = '';
  successMessage = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {

    // Get email saved from Forgot Password page
    const savedEmail = localStorage.getItem('resetEmail');

    if (savedEmail) {
      this.email = savedEmail;
    }
  }

  onSubmit(form: NgForm): void {

    if (form.invalid) {
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.authService.verifyOtp(this.email, this.otp).subscribe({

      next: (response) => {

        console.log('OTP verification response:', response);

        this.isLoading = false;

        // Save email and OTP for Reset Password page
        localStorage.setItem('resetEmail', this.email);
        localStorage.setItem('resetOtp', this.otp);

        this.successMessage =
          'OTP verified successfully.';

        // Go to Reset Password page
        this.router.navigate(['/reset-password']);
      },

      error: (error) => {

        console.error('OTP verification error:', error);

        this.isLoading = false;

        if (error.status === 400 || error.status === 401) {

          this.errorMessage =
            'Invalid or expired OTP. Please try again.';

        } else {

          this.errorMessage =
            'Unable to verify OTP. Please try again.';
        }
      }
    });
  }
}