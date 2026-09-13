import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink
  ],
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.css'
})
export class ForgotPasswordComponent {

  email = '';

  isLoading = false;
  errorMessage = '';
  successMessage = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  onSubmit(form: NgForm): void {

    if (form.invalid) {
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.authService.forgotPassword(this.email).subscribe({

      next: (response) => {

        console.log('Forgot password response:', response);

        this.isLoading = false;

        // Save email for OTP verification
        localStorage.setItem('resetEmail', this.email);

        this.successMessage =
          'OTP has been sent successfully. Please check your email.';

        // Go to Verify OTP page
        this.router.navigate(['/verify-otp']);
      },

      error: (error) => {

        console.error('Forgot password error:', error);

        this.isLoading = false;

        if (error.status === 404) {

          this.errorMessage =
            'Email address not found.';

        } else {

          this.errorMessage =
            'Unable to send OTP. Please try again.';
        }
      }
    });
  }
}