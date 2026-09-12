import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-confirmation-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './confirmation-modal.component.html',
  styleUrl: './confirmation-modal.component.css'
})
export class ConfirmationModalComponent {

  @Input() title = 'Confirm Action';

  @Input() message =
    'Are you sure you want to continue with this action?';

  @Input() confirmText = 'Confirm';

  @Input() cancelText = 'Cancel';

  @Input() type: 'danger' | 'primary' = 'primary';

  @Input() visible = false;

  @Output() confirmed = new EventEmitter<void>();

  @Output() cancelled = new EventEmitter<void>();


  confirm(): void {
    this.confirmed.emit();
  }


  cancel(): void {
    this.cancelled.emit();
  }

}