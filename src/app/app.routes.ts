import { Routes } from '@angular/router';

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
{
  path: 'seeker',
  loadComponent: () =>
    import('./features/seeker/dashboard/dashboard.component')
      .then(m => m.DashboardComponent)
},
{
  path: 'seeker/jobs',
  loadComponent: () =>
    import('./features/seeker/jobs/jobs.component')
      .then(m => m.JobsComponent)
},
{
  path: 'seeker/job-details',
  loadComponent: () =>
    import('./features/seeker/job-details/job-details.component')
      .then(m => m.JobDetailsComponent)
},
{
  path: 'seeker/profile',
  loadComponent: () =>
    import('./features/seeker/profile/profile.component')
      .then(m => m.ProfileComponent)
},
{
  path: 'seeker/applications',
  loadComponent: () =>
    import('./features/seeker/applications/applications.component')
      .then(m => m.ApplicationsComponent)
},
{
  path: 'seeker/notifications',
  loadComponent: () =>
    import('./features/seeker/notifications/notifications.component')
      .then(m => m.NotificationsComponent)
},

{
  path: 'employer',
  loadComponent: () =>
    import('./features/employer/dashboard/dashboard.component')
      .then(m => m.DashboardComponent)
},
{
  path: 'employer/company-profile',
  loadComponent: () =>
    import('./features/employer/company-profile/company-profile.component')
      .then(m => m.CompanyProfileComponent)
},
{
  path: 'employer/vacancies',
  loadComponent: () =>
    import('./features/employer/vacancies/vacancies.component')
      .then(m => m.VacanciesComponent)
},
{
  path: 'employer/applicants',
  loadComponent: () =>
    import('./features/employer/applicants/applicants.component')
      .then(m => m.ApplicantsComponent)
},
{
  path: 'employer/contact-requests',
  loadComponent: () =>
    import('./features/employer/contact-requests/contact-requests.component')
      .then(m => m.ContactRequestsComponent)
},

{
  path: 'employer/create-vacancy',
  loadComponent: () =>
    import('./features/employer/create-vacancy/create-vacancy.component')
      .then(m => m.CreateVacancyComponent)
},
{
  path: 'employer/notifications',
  loadComponent: () =>
    import('./features/employer/notifications/notifications.component')
      .then(m => m.NotificationsComponent)
},

{
  path: 'admin',
  loadComponent: () =>
    import('./features/admin/dashboard/dashboard.component')
      .then(m => m.DashboardComponent)
},
{
  path: 'admin/user-management',
  loadComponent: () =>
    import('./features/admin/user-management/user-management.component')
      .then(m => m.UserManagementComponent)
},
{
  path: 'admin/job-management',
  loadComponent: () =>
    import('./features/admin/job-management/job-management.component')
      .then(m => m.JobManagementComponent)
},
{
  path: 'admin/analytics',
  loadComponent: () =>
    import('./features/admin/analytics/analytics.component')
      .then(m => m.AnalyticsComponent)
},
{
  path: 'admin/notifications',
  loadComponent: () =>
    import('./features/admin/notifications/notifications.component')
      .then(m => m.NotificationsComponent)
},
{
  path: 'admin/settings',
  loadComponent: () =>
    import('./features/admin/settings/settings.component')
      .then(m => m.SettingsComponent)
},
];