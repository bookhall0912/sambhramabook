import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-vendor-settings',
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './settings.component.html',
  styleUrl: './settings.component.scss'
})
export class VendorSettingsComponent implements OnInit {
  pendingBookingsCount = signal<number>(1);

  constructor(
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    window.scrollTo(0, 0);
  }

  onLogout(): void {
    this.authService.logout();
  }
}

