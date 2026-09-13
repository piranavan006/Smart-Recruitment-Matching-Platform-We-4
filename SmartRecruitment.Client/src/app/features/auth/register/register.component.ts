import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  registerData = {
    fullName: '',
    email: '',
    password: '',
    confirmPassword: '',
    role: 'JobSeeker'
  };

  isLoading = false;
  errorMessage = '';
  successMessage = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  onRegister(form: NgForm): void {
    if (form.invalid) {
      return;
    }

    this.errorMessage = '';
    this.successMessage = '';

    if (this.registerData.password !== this.registerData.confirmPassword) {
      this.errorMessage = 'Passwords do not match.';
      return;
    }

    this.isLoading = true;

    const registerRequest = {
      fullName: this.registerData.fullName,
      email: this.registerData.email,
      password: this.registerData.password,
      confirmPassword: this.registerData.confirmPassword,
      role: this.registerData.role
    };

    this.authService.register(registerRequest).subscribe({
      next: (response) => {
        this.isLoading = false;
        this.successMessage = 'Registration successful! Redirecting to login...';

        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 1500);
      },
      error: (error) => {
        this.isLoading = false;
        if (error.status === 400 || error.status === 409) {
          this.errorMessage = error.error?.message || 'Registration failed. Email may already be in use.';
        } else {
          this.errorMessage = 'Unable to connect to the server. Please try again.';
        }
      }
    });
  }
}