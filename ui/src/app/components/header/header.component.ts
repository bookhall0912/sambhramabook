import { Component, effect } from '@angular/core';
import { RouterLink, RouterLinkActive, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-header',
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  isMobileMenuOpen = false;

  constructor(
    private router: Router,
    public authService: AuthService
  ) {
    // Subscribe to auth state changes
    effect(() => {
      // This effect will run whenever auth state changes
      const isAuthenticated = this.authService.isAuthenticated$();
      const userRole = this.authService.userRole$();
      // Header will automatically update via template bindings
    });
  }

  toggleMobileMenu() {
    this.isMobileMenuOpen = !this.isMobileMenuOpen;
  }

  closeMobileMenu() {
    this.isMobileMenuOpen = false;
  }

  navigateToLogin(): void {
    // Get current URL as returnUrl
    const currentUrl = this.router.url;
    this.router.navigate(['/login'], {
      queryParams: { returnUrl: currentUrl }
    });
  }

  navigateToSignup(): void {
    // Get current URL as returnUrl
    const currentUrl = this.router.url;
    this.router.navigate(['/signup'], {
      queryParams: { returnUrl: currentUrl }
    });
  }

  navigateToDashboard(): void {
    const userRole = this.authService.userRole$();
    if (userRole === 'Vendor') {
      this.router.navigate(['/vendor/dashboard']);
    } else if (userRole === 'Admin') {
      this.router.navigate(['/admin/dashboard']);
    } else {
      // Default to landing page for regular users
      this.router.navigate(['/']);
    }
  }

  logout(): void {
    this.authService.logout();
    this.closeMobileMenu();
  }
}
