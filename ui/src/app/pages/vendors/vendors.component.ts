import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { HeaderComponent } from '../../components/header/header.component';
import { FooterComponent } from '../../components/footer/footer.component';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-vendors',
  imports: [CommonModule, HeaderComponent, FooterComponent],
  templateUrl: './vendors.component.html',
  styleUrl: './vendors.component.scss'
})
export class VendorsComponent {
  constructor(
    private router: Router,
    public authService: AuthService
  ) {}

  onGetStarted(): void {
    // Navigate to login with vendor context
    // Redirect to vendor dashboard after login
    this.router.navigate(['/login'], {
      queryParams: { 
        returnUrl: '/vendor/dashboard',
        vendor: 'true' // Flag to show vendor-specific messaging
      }
    });
  }

  onLogin(): void {
    // If already logged in as vendor, go to dashboard
    if (this.authService.isAuthenticated$() && this.authService.userRole$() === 'Vendor') {
      this.router.navigate(['/vendor/dashboard']);
    } else {
      this.router.navigate(['/login'], {
        queryParams: { 
          returnUrl: '/vendor/dashboard',
          vendor: 'true'
        }
      });
    }
  }
}

