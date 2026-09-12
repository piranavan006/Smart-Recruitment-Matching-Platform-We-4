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
    role: ''
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

    // Check password confirmation
    if (this.registerData.password !== this.registerData.confirmPassword) {

      this.errorMessage = 'Passwords do not match.';

      return;
    }

    this.isLoading = true;

    // Data sent to backend
    const registerRequest = {
      fullName: this.registerData.fullName,
      email: this.registerData.email,
      password: this.registerData.password,
      role: this.registerData.role
    };

    console.log('Register Request:', registerRequest);

    this.authService.register(registerRequest).subscribe({

      next: (response) => {

        console.log('Registration successful:', response);

        this.isLoading = false;

        this.successMessage =
          'Registration successful! You can now login.';

        // Clear form
        this.registerData = {
          fullName: '',
          email: '',
          password: '',
          confirmPassword: '',
          role: ''
        };

      },

      error: (error) => {

        console.error('Registration error:', error);

        this.isLoading = false;

        if (error.status === 400) {

          this.errorMessage =
            error.error?.message ||
            'Registration failed. Please check your details.';

        } else {

          this.errorMessage =
            'Unable to connect to the server. Please try again.';
        }
      }

    });
  }
}