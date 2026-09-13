import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
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
    private router: Router,
    private route: ActivatedRoute
  ) {}

  onLogin(form: NgForm): void {
    if (form.invalid) {
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    this.authService.login(this.loginData).subscribe({
      next: (response) => {
        this.isLoading = false;

        const returnUrl = this.route.snapshot.queryParams['returnUrl'];
        if (returnUrl) {
          this.router.navigateByUrl(returnUrl);
          return;
        }

        const role = response.role?.toLowerCase();
        if (role === 'admin' || role === 'administrator') {
          this.router.navigate(['/admin']);
        } else if (role === 'employer') {
          this.router.navigate(['/employer']);
        } else {
          this.router.navigate(['/seeker']);
        }
      },
      error: (error) => {
        this.isLoading = false;
        if (error.status === 401) {
          this.errorMessage = error.error?.message || 'Invalid email or password.';
        } else if (error.status === 0) {
          this.errorMessage = 'Cannot reach backend server. Please ensure the API is running.';
        } else {
          this.errorMessage = error.error?.message || 'Unable to log in. Please try again.';
        }
      }
    });
  }
}