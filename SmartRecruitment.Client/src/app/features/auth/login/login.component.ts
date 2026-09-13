import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { User } from '../../../core/models/auth.model';

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
export class LoginComponent implements OnInit {
  loginData = {
    email: '',
    password: ''
  };

  isLoading = false;
  errorMessage = '';
  loggedInUser: User | null = null;

  constructor(
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.loggedInUser = this.authService.getCurrentUser();
  }

  goToDashboard(): void {
    if (!this.loggedInUser) return;
    const role = (this.loggedInUser.role || '').toLowerCase();
    if (role === 'admin' || role === 'administrator') {
      this.router.navigate(['/admin']);
    } else if (role === 'employer') {
      this.router.navigate(['/employer']);
    } else {
      this.router.navigate(['/seeker']);
    }
  }

  switchAccount(): void {
    this.authService.logout();
    this.loggedInUser = null;
    this.loginData.email = '';
    this.loginData.password = '';
  }

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