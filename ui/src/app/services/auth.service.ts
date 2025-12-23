import { Injectable, signal, computed, inject } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { environment } from 'environments/environment';

export interface User {
  id: string;
  email?: string;
  mobile?: string;
  name: string;
  role: 'User' | 'Vendor' | 'Admin';
  [key: string]: any; // For additional user properties
}

export interface AuthState {
  isAuthenticated: boolean;
  user: User | null;
  token: string | null;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly authState = signal<AuthState>({
    isAuthenticated: false,
    user: null,
    token: null
  });

  // Public signals
  public readonly isAuthenticated$ = computed(() => this.authState().isAuthenticated);
  public readonly currentUser$ = computed(() => this.authState().user);
  public readonly userRole$ = computed(() => this.authState().user?.role || null);

  constructor(private router: Router) {
    // Load auth state from localStorage on initialization
    this.loadAuthState();
  }

  /**
   * Login user with JWT token
   * In the future, this will be called after OTP verification
   * Note: Navigation is handled by the calling component if returnUrl is provided
   */
  login(token: string, user: User): void {
    this.authState.set({
      isAuthenticated: true,
      user: user,
      token: token
    });

    // Save to localStorage
    localStorage.setItem('auth_token', token);
    localStorage.setItem('user_data', JSON.stringify(user));

    // Navigation is handled by the calling component (login/register) to respect returnUrl
    // Only navigate here if no returnUrl handling is needed
  }

  /**
   * Logout user
   */
  logout(): void {
    this.authState.set({
      isAuthenticated: false,
      user: null,
      token: null
    });

    // Clear localStorage
    localStorage.removeItem('auth_token');
    localStorage.removeItem('user_data');

    // Navigate to landing page
    this.router.navigate(['/']);
  }

  /**
   * Get current JWT token
   */
  getToken(): string | null {
    return this.authState().token;
  }

  /**
   * Check if user has specific role
   */
  hasRole(role: 'User' | 'Vendor' | 'Admin'): boolean {
    return this.authState().user?.role === role;
  }

  /**
   * Navigate user based on their role
   * Note: This is only called if no returnUrl is provided during login
   */
  private navigateByRole(role: 'User' | 'Vendor' | 'Admin'): void {
    switch (role) {
      case 'User':
        this.router.navigate(['/']);
        break;
      case 'Vendor':
        this.router.navigate(['/vendor/dashboard']);
        break;
      case 'Admin':
        this.router.navigate(['/admin/dashboard']);
        break;
      default:
        this.router.navigate(['/']);
    }
  }

  /**
   * Load auth state from localStorage
   */
  private loadAuthState(): void {
    const token = localStorage.getItem('auth_token');
    const userData = localStorage.getItem('user_data');

    if (token && userData) {
      try {
        const user = JSON.parse(userData);
        this.authState.set({
          isAuthenticated: true,
          user: user,
          token: token
        });
      } catch (error) {
        console.error('Error parsing user data from localStorage:', error);
        this.clearAuthState();
      }
    }
  }

  /**
   * Clear auth state (used on error)
   */
  private clearAuthState(): void {
    localStorage.removeItem('auth_token');
    localStorage.removeItem('user_data');
    this.authState.set({
      isAuthenticated: false,
      user: null,
      token: null
    });
  }

  /**
   * Send OTP to mobile/email
   */
  async sendOtp(mobileOrEmail: string): Promise<{ otpSent: boolean; message: string; expiresIn?: number }> {
    try {
      const response = await firstValueFrom(
        this.http.post<{ success: boolean; data: { otpSent: boolean; message: string; expiresIn: number } }>(
          `${environment.apiUrl}/auth/send-otp`,
          { mobileOrEmail }
        )
      );
      return response.data || { otpSent: false, message: 'Failed to send OTP' };
    } catch (error: any) {
      console.error('Error sending OTP:', error);
      throw new Error(error?.error?.error?.message || 'Failed to send OTP');
    }
  }

  /**
   * Verify OTP and login
   */
  async verifyOtp(mobileOrEmail: string, otp: string): Promise<{ token: string; user: User }> {
    try {
      const response = await firstValueFrom(
        this.http.post<{ success: boolean; data: { token: string; refreshToken: string; user: User } }>(
          `${environment.apiUrl}/auth/verify-otp`,
          { mobileOrEmail, otp }
        )
      );
      
      if (response.success && response.data) {
        // Store refresh token for future use
        if (response.data.refreshToken) {
          localStorage.setItem('refresh_token', response.data.refreshToken);
        }
        
        return {
          token: response.data.token,
          user: response.data.user
        };
      }
      
      throw new Error('Invalid response from server');
    } catch (error: any) {
      console.error('Error verifying OTP:', error);
      throw new Error(error?.error?.error?.message || 'Failed to verify OTP');
    }
  }
}

