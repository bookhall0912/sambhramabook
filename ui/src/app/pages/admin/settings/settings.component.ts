import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-admin-settings',
  imports: [CommonModule, FormsModule, RouterLink, RouterLinkActive],
  templateUrl: './settings.component.html',
  styleUrl: './settings.component.scss'
})
export class AdminSettingsComponent implements OnInit {
  pendingCount = signal<number>(5);
  payoutsCount = signal<number>(2);
  
  settings = signal({
    platformName: 'SambhramaBook',
    commissionRate: 15,
    minBookingAmount: 10000,
    maxBookingAmount: 1000000,
    supportEmail: 'support@sambhramabook.com',
    supportPhone: '+91 98765 43210',
    enableNotifications: true,
    enableEmailAlerts: true
  });

  constructor(
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    window.scrollTo(0, 0);
    if (!this.authService.hasRole('Admin')) {
      this.router.navigate(['/']);
    }
  }

  updatePlatformName(event: Event): void {
    const value = (event.target as HTMLInputElement).value;
    this.settings.update(s => ({ ...s, platformName: value }));
  }

  updateCommissionRate(event: Event): void {
    const value = +(event.target as HTMLInputElement).value;
    this.settings.update(s => ({ ...s, commissionRate: value }));
  }

  updateMinBookingAmount(event: Event): void {
    const value = +(event.target as HTMLInputElement).value;
    this.settings.update(s => ({ ...s, minBookingAmount: value }));
  }

  updateMaxBookingAmount(event: Event): void {
    const value = +(event.target as HTMLInputElement).value;
    this.settings.update(s => ({ ...s, maxBookingAmount: value }));
  }

  updateSupportEmail(event: Event): void {
    const value = (event.target as HTMLInputElement).value;
    this.settings.update(s => ({ ...s, supportEmail: value }));
  }

  updateSupportPhone(event: Event): void {
    const value = (event.target as HTMLInputElement).value;
    this.settings.update(s => ({ ...s, supportPhone: value }));
  }

  updateEnableNotifications(event: Event): void {
    const checked = (event.target as HTMLInputElement).checked;
    this.settings.update(s => ({ ...s, enableNotifications: checked }));
  }

  updateEnableEmailAlerts(event: Event): void {
    const checked = (event.target as HTMLInputElement).checked;
    this.settings.update(s => ({ ...s, enableEmailAlerts: checked }));
  }

  onSave(): void {
    console.log('Saving settings:', this.settings());
    // TODO: Call API to save settings
  }

  onLogout(): void {
    this.authService.logout();
  }
}

