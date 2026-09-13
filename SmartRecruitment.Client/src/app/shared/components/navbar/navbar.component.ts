import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterLinkActive, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
import { AuthService } from '../../../core/services/auth.service';
import { User } from '../../../core/models/auth.model';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit {
  currentUser: User | null = null;
  mobileMenuOpen = false;
  currentUrl = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.currentUrl = this.router.url;

    this.router.events.pipe(
      filter((event): event is NavigationEnd => event instanceof NavigationEnd)
    ).subscribe((event: NavigationEnd) => {
      this.currentUrl = event.urlAfterRedirects || event.url;
    });

    this.authService.currentUser$.subscribe(user => {
      this.currentUser = user;
    });
  }

  get isAuthRoute(): boolean {
    const cleanUrl = (this.currentUrl || this.router.url || '').split('?')[0].toLowerCase();
    return cleanUrl === '/login' ||
           cleanUrl === '/register' ||
           cleanUrl === '/forgot-password' ||
           cleanUrl === '/verify-otp' ||
           cleanUrl === '/reset-password' ||
           cleanUrl === '/' ||
           cleanUrl === '';
  }

  get isJobSeeker(): boolean {
    return this.currentUser?.role?.toLowerCase() === 'jobseeker';
  }

  get isEmployer(): boolean {
    return this.currentUser?.role?.toLowerCase() === 'employer';
  }

  get isAdmin(): boolean {
    const role = this.currentUser?.role?.toLowerCase();
    return role === 'admin' || role === 'administrator';
  }

  get isLoggedIn(): boolean {
    return !this.isAuthRoute && this.authService.isLoggedIn() && !!this.currentUser;
  }

  toggleMobileMenu(): void {
    this.mobileMenuOpen = !this.mobileMenuOpen;
  }

  closeMobileMenu(): void {
    this.mobileMenuOpen = false;
  }

  logout(): void {
    this.authService.logout();
    this.closeMobileMenu();
    this.router.navigate(['/login']);
  }
}
