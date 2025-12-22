import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface Tab {
  id: string;
  label: string;
}

@Component({
  selector: 'app-tabs',
  imports: [CommonModule],
  templateUrl: './tabs.component.html',
  styleUrl: './tabs.component.scss'
})
export class TabsComponent {
  tabs = input.required<Tab[]>();
  activeTab = input<string>('');
  tabChanged = output<string>();

  onTabClick(tabId: string): void {
    this.tabChanged.emit(tabId);
  }
}

