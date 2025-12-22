import { Injectable, signal, computed } from '@angular/core';
import { Router } from '@angular/router';

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
   * Verify OTP and login (mock implementation for now)
   * In the future, this will call the actual API
   */
  async verifyOtp(mobileOrEmail: string, otp: string): Promise<{ token: string; user: User }> {
    // TODO: Replace with actual API call
    // const response = await this.http.post<{ token: string; user: User }>('/api/auth/verify-otp', {
    //   mobileOrEmail,
    //   otp
    // }).toPromise();
    // return response;

    // Mock implementation for now
    return new Promise((resolve) => {
      setTimeout(() => {
        // Mock user data - in real implementation, this comes from API
        // For testing: specific phone numbers for different roles
        // Clean phone number (remove all non-digits) for storage and role checking
        const cleanPhone = mobileOrEmail.replace(/\D/g, '');
        const isEmail = mobileOrEmail.includes('@');
        const isAdmin = cleanPhone === '9876543210';
        const isVendor = cleanPhone === '9876543211';
        
        let role: 'User' | 'Vendor' | 'Admin' = 'User';
        if (isAdmin) {
          role = 'Admin';
        } else if (isVendor) {
          role = 'Vendor';
        }
        
        const mockUser: User = {
          id: 'user-' + Date.now(),
          email: isEmail ? mobileOrEmail : undefined,
          mobile: isEmail ? undefined : cleanPhone, // Store cleaned phone number
          name: role === 'Admin' ? 'Admin User' : role === 'Vendor' ? 'Vendor User' : 'User',
          role: role
        };

        const mockToken = 'mock_jwt_token_' + Date.now();
        resolve({ token: mockToken, user: mockUser });
      }, 500);
    });
  }
}

