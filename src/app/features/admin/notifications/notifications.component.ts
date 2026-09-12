import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

interface AdminNotification {
  id: number;
  icon: string;
  type: string;
  title: string;
  message: string;
  time: string;
  isRead: boolean;
}

@Component({
  selector: 'app-admin-notifications',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink
  ],
  templateUrl: './notifications.component.html',
  styleUrl: './notifications.component.css'
})
export class NotificationsComponent {

  /* =====================================================
     NOTIFICATIONS
     ===================================================== */

  notifications: AdminNotification[] = [

    {
      id: 1,
      icon: '👤',
      type: 'User',
      title: 'New User Registered',
      message:
        'A new job seeker account has been successfully created on the platform.',
      time: '10 minutes ago',
      isRead: false
    },

    {
      id: 2,
      icon: '💼',
      type: 'Job',
      title: 'New Vacancy Posted',
      message:
        'Tech Solutions has posted a new Software Engineer vacancy.',
      time: '35 minutes ago',
      isRead: false
    },

    {
      id: 3,
      icon: '⭐',
      type: 'Matching',
      title: 'High Match Detected',
      message:
        'A candidate achieved a high matching result for an available vacancy.',
      time: '1 hour ago',
      isRead: false
    },

    {
      id: 4,
      icon: '📄',
      type: 'Application',
      title: 'New Application Received',
      message:
        'A new candidate application has been submitted for a vacancy.',
      time: '2 hours ago',
      isRead: true
    },

    {
      id: 5,
      icon: '👥',
      type: 'User',
      title: 'User Account Updated',
      message:
        'A platform user has updated their account information.',
      time: '4 hours ago',
      isRead: true
    },

    {
      id: 6,
      icon: '🔒',
      type: 'Security',
      title: 'Account Status Changed',
      message:
        'An administrator changed the status of a user account.',
      time: 'Yesterday',
      isRead: true
    },

    {
      id: 7,
      icon: '📊',
      type: 'System',
      title: 'Platform Statistics Updated',
      message:
        'Recruitment activity statistics have been refreshed.',
      time: 'Yesterday',
      isRead: true
    }

  ];


  /* =====================================================
     FILTER
     ===================================================== */

  selectedFilter = 'All';

  filters: string[] = [
    'All',
    'Unread',
    'User',
    'Job',
    'Application',
    'Matching',
    'Security',
    'System'
  ];


  /* =====================================================
     FILTERED NOTIFICATIONS
     ===================================================== */

  get filteredNotifications(): AdminNotification[] {

    return this.notifications.filter(notification => {

      if (this.selectedFilter === 'All') {
        return true;
      }

      if (this.selectedFilter === 'Unread') {
        return !notification.isRead;
      }

      return notification.type === this.selectedFilter;

    });

  }


  /* =====================================================
     UNREAD COUNT
     ===================================================== */

  getUnreadCount(): number {

    return this.notifications.filter(
      notification => !notification.isRead
    ).length;

  }


  /* =====================================================
     TOTAL COUNT
     ===================================================== */

  getTotalCount(): number {

    return this.notifications.length;

  }


  /* =====================================================
     MARK SINGLE AS READ
     ===================================================== */

  markAsRead(notification: AdminNotification): void {

    notification.isRead = true;

  }


  /* =====================================================
     MARK ALL AS READ
     ===================================================== */

  markAllAsRead(): void {

    this.notifications.forEach(
      notification => notification.isRead = true
    );

  }


  /* =====================================================
     DELETE NOTIFICATION
     ===================================================== */

  removeNotification(
    notification: AdminNotification
  ): void {

    this.notifications =
      this.notifications.filter(
        item => item.id !== notification.id
      );

  }


  /* =====================================================
     CHANGE FILTER
     ===================================================== */

  changeFilter(filter: string): void {

    this.selectedFilter = filter;

  }

}