import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NotificationService } from '../../../core/services/notification.service';
import { AuthService } from '../../../core/services/auth.service';
import { Notification } from '../../../core/models/notification.model';

export interface EmployerNotificationItem {
  id: number;
  title: string;
  message: string;
  time: string;
  icon: string;
  isRead: boolean;
  raw: Notification;
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
export class NotificationsComponent implements OnInit {
  notifications: EmployerNotificationItem[] = [];
  isLoading = true;

  constructor(
    private notificationService: NotificationService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    const user = this.authService.getCurrentUser();
    if (user) {
      this.loadNotifications(user.userId);
    } else {
      this.isLoading = false;
    }
  }

  loadNotifications(userId: number): void {
    this.isLoading = true;
    this.notificationService.getUserNotifications(userId).subscribe({
      next: (data) => {
        this.notifications = (data || []).map(n => ({
          id: n.notificationId,
          title: 'Notification Alert',
          message: n.message,
          time: n.createdAt ? new Date(n.createdAt).toLocaleDateString() : 'Recent',
          icon: '🔔',
          isRead: n.isRead,
          raw: n
        }));
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }

  getUnreadCount(): number {
    return this.notifications.filter(n => !n.isRead).length;
  }

  markAsRead(notification: EmployerNotificationItem): void {
    notification.isRead = true;
    notification.raw.isRead = true;
    this.notificationService.markAsRead(notification.raw).subscribe({
      error: () => {}
    });
  }

  markAllAsRead(): void {
    this.notifications.forEach(item => {
      if (!item.isRead) {
        this.markAsRead(item);
      }
    });
  }
}