import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

interface NotificationItem {
  title: string;
  message: string;
  time: string;
  icon: string;
  isRead: boolean;
}

@Component({
  selector: 'app-employer-notifications',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink
  ],
  templateUrl: './notifications.component.html',
  styleUrl: './notifications.component.css'
})
export class NotificationsComponent {

  notifications: NotificationItem[] = [

    {
      title: 'New Application Received',
      message:
        'A new candidate has applied for your Software Engineer vacancy.',
      time: '1 hour ago',
      icon: '📄',
      isRead: false
    },

    {
      title: 'New Candidate Match',
      message:
        'A candidate with a high matching score has been found for your vacancy.',
      time: '3 hours ago',
      icon: '⭐',
      isRead: false
    },

    {
      title: 'New Contact Request',
      message:
        'A candidate has sent a request to contact your company regarding a job opportunity.',
      time: '1 day ago',
      icon: '📩',
      isRead: false
    },

    {
      title: 'Application Status Updated',
      message:
        'An applicant status has been updated for your Frontend Developer vacancy.',
      time: '2 days ago',
      icon: '🔔',
      isRead: true
    },

    {
      title: 'Vacancy Reminder',
      message:
        'Your active vacancies are available for review and management.',
      time: '3 days ago',
      icon: '💼',
      isRead: true
    }

  ];


  // Get unread notification count
  getUnreadCount(): number {

    return this.notifications.filter(
      notification => !notification.isRead
    ).length;

  }


  // Mark one notification as read
  markAsRead(notification: NotificationItem): void {

    notification.isRead = true;

  }


  // Mark all notifications as read
  markAllAsRead(): void {

    this.notifications.forEach(
      notification => notification.isRead = true
    );

  }

}