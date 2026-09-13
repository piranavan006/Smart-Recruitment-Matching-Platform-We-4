import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-empty-state',
  standalone: true,
  templateUrl: './empty-state.component.html',
  styleUrl: './empty-state.component.css'
})
export class EmptyStateComponent {

  @Input() icon = '📭';

  @Input() title = 'No Data Found';

  @Input() message = 'There is no information available at the moment.';

}