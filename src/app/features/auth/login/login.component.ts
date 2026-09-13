import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  loginData = {
    email: '',
    password: ''
  };

  isLoading = false;
  errorMessage = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  onLogin(form: NgForm): void {

    if (form.invalid) {
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    this.authService.login(this.loginData).subscribe({

      next: (response) => {

        console.log('Login successful:', response);

        // Save JWT token
        localStorage.setItem('token', response.token);

        // Save user information
        localStorage.setItem('userId', response.userId.toString());
        localStorage.setItem('fullName', response.fullName);
        localStorage.setItem('email', response.email);
        localStorage.setItem('role', response.role);

        this.isLoading = false;

        // Navigate to role-specific dashboard
        if (response.role === 'Admin') {
          this.router.navigate(['/admin']);
        } else if (response.role === 'Employer') {
          this.router.navigate(['/employer']);
        } else {
          this.router.navigate(['/seeker']);
        }
      },

      error: (error) => {

        console.error('Login error:', error);

        this.isLoading = false;

        if (error.status === 401) {
          this.errorMessage = 'Invalid email or password.';
        } else {
          this.errorMessage = 'Unable to connect to the server.';
        }
      }
    });
  }
}