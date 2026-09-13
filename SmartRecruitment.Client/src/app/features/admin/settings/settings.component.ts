import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-admin-settings',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink
  ],
  templateUrl: './settings.component.html',
  styleUrl: './settings.component.css'
})
export class SettingsComponent {

  /* =====================================================
     PLATFORM SETTINGS
     ===================================================== */

  platformName = 'SmartRecruitment';

  platformEmail = 'admin@smartrecruitment.com';

  maintenanceMode = false;

  allowNewRegistrations = true;

  emailNotifications = true;

  applicationNotifications = true;

  matchingNotifications = true;


  /* =====================================================
     SECURITY SETTINGS
     ===================================================== */

  sessionTimeout = 60;

  minimumPasswordLength = 8;

  requireStrongPassword = true;

  enableLoginProtection = true;


  /* =====================================================
     MATCHING SETTINGS
     ===================================================== */

  minimumMatchScore = 60;

  showMatchScoreToEmployer = true;

  showMatchScoreToSeeker = true;


  /* =====================================================
     SYSTEM INFORMATION
     ===================================================== */

  systemVersion = '1.0.0';

  angularVersion = '19';

  backendVersion = 'ASP.NET Core 8';

  database = 'SQL Server';


  /* =====================================================
     SAVE SETTINGS
     ===================================================== */

  saveSettings(): void {

    console.log('Admin settings saved successfully.');

    alert('Settings saved successfully.');

  }


  /* =====================================================
     RESET SETTINGS
     ===================================================== */

  resetSettings(): void {

    this.platformName = 'SmartRecruitment';

    this.platformEmail = 'admin@smartrecruitment.com';

    this.maintenanceMode = false;

    this.allowNewRegistrations = true;

    this.emailNotifications = true;

    this.applicationNotifications = true;

    this.matchingNotifications = true;

    this.sessionTimeout = 60;

    this.minimumPasswordLength = 8;

    this.requireStrongPassword = true;

    this.enableLoginProtection = true;

    this.minimumMatchScore = 60;

    this.showMatchScoreToEmployer = true;

    this.showMatchScoreToSeeker = true;

  }


  /* =====================================================
     TOGGLE MAINTENANCE
     ===================================================== */

  toggleMaintenance(): void {

    this.maintenanceMode = !this.maintenanceMode;

  }


  /* =====================================================
     TOGGLE REGISTRATION
     ===================================================== */

  toggleRegistrations(): void {

    this.allowNewRegistrations =
      !this.allowNewRegistrations;

  }

}