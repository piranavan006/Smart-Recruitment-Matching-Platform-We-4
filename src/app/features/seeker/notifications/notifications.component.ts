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
  selector: 'app-notifications',
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
      title: 'Application Submitted',
      message: 'Your application for Software Engineer at Tech Solutions was submitted successfully.',
      time: '2 hours ago',
      icon: '📄',
      isRead: false
    },
    {
      title: 'Application Accepted',
      message: 'Congratulations! Your application for Frontend Developer has been accepted.',
      time: '1 day ago',
      icon: '🎉',
      isRead: false
    },
    {
      title: 'New Job Match',
      message: 'A new job matching your skills is now available.',
      time: '2 days ago',
      icon: '⭐',
      isRead: true
    },
    {
      title: 'Profile Reminder',
      message: 'Complete your profile to improve your job matching results.',
      time: '3 days ago',
      icon: '👤',
      isRead: true
    }
  ];

  getUnreadCount(): number {
    return this.notifications.filter(
      notification => !notification.isRead
    ).length;
  }

  markAsRead(notification: NotificationItem): void {
    notification.isRead = true;
  }

  markAllAsRead(): void {
    this.notifications.forEach(
      notification => notification.isRead = true
    );
  }

}