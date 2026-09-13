import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NotificationService } from '../../../core/services/notification.service';
import { AuthService } from '../../../core/services/auth.service';
import { Notification } from '../../../core/models/notification.model';

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
export class NotificationsComponent implements OnInit {
  notifications: Notification[] = [];
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
        this.notifications = data;
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

  markAsRead(item: Notification): void {
    item.isRead = true;
    this.notificationService.markAsRead(item).subscribe({
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