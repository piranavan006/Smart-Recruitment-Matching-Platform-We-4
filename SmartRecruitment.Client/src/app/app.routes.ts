import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { roleGuard } from './core/guards/role.guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  {
    path: 'login',
    loadComponent: () =>
      import('./features/auth/login/login.component')
        .then(m => m.LoginComponent)
  },
  {
    path: 'register',
    loadComponent: () =>
      import('./features/auth/register/register.component')
        .then(m => m.RegisterComponent)
  },
  {
    path: 'forgot-password',
    loadComponent: () =>
      import('./features/auth/forgot-password/forgot-password.component')
        .then(m => m.ForgotPasswordComponent)
  },
  {
    path: 'verify-otp',
    loadComponent: () =>
      import('./features/auth/verify-otp/verify-otp.component')
        .then(m => m.VerifyOtpComponent)
  },
  {
    path: 'reset-password',
    loadComponent: () =>
      import('./features/auth/reset-password/reset-password.component')
        .then(m => m.ResetPasswordComponent)
  },

  // Job Seeker Routes (Protected)
  {
    path: 'seeker',
    canActivate: [authGuard, roleGuard],
    data: { roles: ['JobSeeker'] },
    loadComponent: () =>
      import('./features/seeker/dashboard/dashboard.component')
        .then(m => m.DashboardComponent)
  },
  {
    path: 'seeker/jobs',
    canActivate: [authGuard, roleGuard],
    data: { roles: ['JobSeeker'] },
    loadComponent: () =>
      import('./features/seeker/jobs/jobs.component')
        .then(m => m.JobsComponent)
  },
  {
    path: 'seeker/job-details/:id',
    canActivate: [authGuard, roleGuard],
    data: { roles: ['JobSeeker', 'Admin', 'Administrator', 'Employer'] },
    loadComponent: () =>
      import('./features/seeker/job-details/job-details.component')
        .then(m => m.JobDetailsComponent)
  },
  {
    path: 'seeker/job-details',
    canActivate: [authGuard, roleGuard],
    data: { roles: ['JobSeeker', 'Admin', 'Administrator', 'Employer'] },
    loadComponent: () =>
      import('./features/seeker/job-details/job-details.component')
        .then(m => m.JobDetailsComponent)
  },
  {
    path: 'seeker/profile',
    canActivate: [authGuard, roleGuard],
    data: { roles: ['JobSeeker'] },
    loadComponent: () =>
      import('./features/seeker/profile/profile.component')
        .then(m => m.ProfileComponent)
  },
  {
    path: 'seeker/applications',
    canActivate: [authGuard, roleGuard],
    data: { roles: ['JobSeeker'] },
    loadComponent: () =>
      import('./features/seeker/applications/applications.component')
        .then(m => m.ApplicationsComponent)
  },
  {
    path: 'seeker/notifications',
    canActivate: [authGuard, roleGuard],
    data: { roles: ['JobSeeker'] },
    loadComponent: () =>
      import('./features/seeker/notifications/notifications.component')
        .then(m => m.NotificationsComponent)
  },

  // Employer Routes (Protected)
  {
    path: 'employer',
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Employer'] },
    loadComponent: () =>
      import('./features/employer/dashboard/dashboard.component')
        .then(m => m.DashboardComponent)
  },
  {
    path: 'employer/company-profile',
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Employer'] },
    loadComponent: () =>
      import('./features/employer/company-profile/company-profile.component')
        .then(m => m.CompanyProfileComponent)
  },
  {
    path: 'employer/vacancies',
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Employer'] },
    loadComponent: () =>
      import('./features/employer/vacancies/vacancies.component')
        .then(m => m.VacanciesComponent)
  },
  {
    path: 'employer/applicants',
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Employer'] },
    loadComponent: () =>
      import('./features/employer/applicants/applicants.component')
        .then(m => m.ApplicantsComponent)
  },
  {
    path: 'employer/contact-requests',
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Employer'] },
    loadComponent: () =>
      import('./features/employer/contact-requests/contact-requests.component')
        .then(m => m.ContactRequestsComponent)
  },
  {
    path: 'employer/create-vacancy',
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Employer'] },
    loadComponent: () =>
      import('./features/employer/create-vacancy/create-vacancy.component')
        .then(m => m.CreateVacancyComponent)
  },
  {
    path: 'employer/notifications',
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Employer'] },
    loadComponent: () =>
      import('./features/employer/notifications/notifications.component')
        .then(m => m.NotificationsComponent)
  },

  // Admin Routes (Protected)
  {
    path: 'admin',
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Admin', 'Administrator'] },
    loadComponent: () =>
      import('./features/admin/dashboard/dashboard.component')
        .then(m => m.DashboardComponent)
  },
  {
    path: 'admin/user-management',
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Admin', 'Administrator'] },
    loadComponent: () =>
      import('./features/admin/user-management/user-management.component')
        .then(m => m.UserManagementComponent)
  },
  {
    path: 'admin/job-management',
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Admin', 'Administrator'] },
    loadComponent: () =>
      import('./features/admin/job-management/job-management.component')
        .then(m => m.JobManagementComponent)
  },
  {
    path: 'admin/analytics',
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Admin', 'Administrator'] },
    loadComponent: () =>
      import('./features/admin/analytics/analytics.component')
        .then(m => m.AnalyticsComponent)
  },
  {
    path: 'admin/notifications',
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Admin', 'Administrator'] },
    loadComponent: () =>
      import('./features/admin/notifications/notifications.component')
        .then(m => m.NotificationsComponent)
  },
  {
    path: 'admin/settings',
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Admin', 'Administrator'] },
    loadComponent: () =>
      import('./features/admin/settings/settings.component')
        .then(m => m.SettingsComponent)
  },

  // Catch-all
  {
    path: '**',
    redirectTo: 'login'
  }
];