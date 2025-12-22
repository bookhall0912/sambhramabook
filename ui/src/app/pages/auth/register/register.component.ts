import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, FormArray, FormControl } from '@angular/forms';
import { Router, RouterLink, ActivatedRoute } from '@angular/router';
import { AuthService } from '../../../services/auth.service';

type UserRole = 'Customer' | 'Vendor';
type VendorType = 'Hall Owner' | 'Service Provider';

@Component({
  selector: 'app-register',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  otpForm: FormGroup;
  selectedRole = signal<UserRole>('Vendor');
  otpSent = signal<boolean>(false);

  private returnUrl: string | null = null;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private authService: AuthService
  ) {
    this.registerForm = this.fb.group({
      fullName: ['', [Validators.required]],
      mobileNumber: ['', [Validators.required, Validators.pattern(/^[\d\s]{10,}$/)]],
      email: ['', [Validators.required, Validators.email]],
      // Business details (only for vendors)
      businessName: [''],
      vendorType: ['Hall Owner'],
      // Terms acceptance
      acceptTerms: [false, [Validators.requiredTrue]]
    });

    // Create OTP form with 6 inputs
    const otpControls = Array.from({ length: 6 }, () => 
      this.fb.control('', [Validators.required, Validators.pattern(/[0-9]/)])
    );
    this.otpForm = this.fb.group({
      otp: this.fb.array(otpControls)
    });
  }

  ngOnInit(): void {
    // Scroll to top when component initializes
    window.scrollTo(0, 0);

    // Get returnUrl from query params
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || null;

    // Update form validation based on role
    this.updateFormValidation();
  }

  selectRole(role: UserRole): void {
    this.selectedRole.set(role);
    this.updateFormValidation();
  }

  private updateFormValidation(): void {
    const businessNameControl = this.registerForm.get('businessName');
    const vendorTypeControl = this.registerForm.get('vendorType');

    if (this.selectedRole() === 'Vendor') {
      businessNameControl?.setValidators([Validators.required]);
      vendorTypeControl?.setValidators([Validators.required]);
    } else {
      businessNameControl?.clearValidators();
      vendorTypeControl?.clearValidators();
      businessNameControl?.setValue('');
    }

    businessNameControl?.updateValueAndValidity();
    vendorTypeControl?.updateValueAndValidity();
  }

  onMobileInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    let value = input.value;

    // Remove any non-digit characters (allow only digits and spaces)
    value = value.replace(/[^\d\s]/g, '');

    // Update input value
    if (input.value !== value) {
      input.value = value;
      this.registerForm.get('mobileNumber')?.setValue(value, { emitEvent: false });
    }
  }

  onMobileKeyDown(event: KeyboardEvent): void {
    // Allow: backspace, delete, tab, escape, enter, arrow keys, numbers, and space
    const allowedKeys = [
      'Backspace', 'Delete', 'Tab', 'Escape', 'Enter',
      'ArrowLeft', 'ArrowRight', 'ArrowUp', 'ArrowDown', ' ',
      'Home', 'End'
    ];
    
    // Block non-numeric and non-allowed keys
    if (!allowedKeys.includes(event.key) && !/[0-9]/.test(event.key)) {
      event.preventDefault();
    }
  }

  get otpArray(): FormArray {
    return this.otpForm.get('otp') as FormArray;
  }

  getOtpControl(index: number): FormControl {
    return this.otpArray.at(index) as FormControl;
  }

  async onSendOtp(): Promise<void> {
    if (this.registerForm.valid) {
      const mobileNumber = this.registerForm.get('mobileNumber')?.value;
      // TODO: Call API to send OTP
      console.log('Sending OTP to:', mobileNumber);
      this.otpSent.set(true);
      // Focus first OTP input after a short delay
      setTimeout(() => {
        const firstInput = document.getElementById('otp-0') as HTMLInputElement;
        if (firstInput) {
          firstInput.focus();
        }
      }, 100);
    } else {
      this.registerForm.markAllAsTouched();
    }
  }

  onChangePhone(): void {
    this.otpSent.set(false);
    this.otpForm.reset();
    // Reset all OTP inputs
    this.otpArray.controls.forEach(control => control.setValue(''));
  }

  onOtpInput(event: Event, index: number): void {
    const input = event.target as HTMLInputElement;
    let value = input.value;

    // Remove any non-digit characters
    value = value.replace(/\D/g, '');

    // Only allow single digit
    if (value.length > 1) {
      value = value.slice(-1);
    }

    // Update input and form control
    input.value = value;
    this.getOtpControl(index).setValue(value);

    // Move to next input if value entered
    if (value && index < 5) {
      const nextInput = document.getElementById(`otp-${index + 1}`) as HTMLInputElement;
      if (nextInput) {
        nextInput.focus();
      }
    }
  }

  onOtpKeyDown(event: KeyboardEvent, index: number): void {
    const input = event.target as HTMLInputElement;

    // Allow: backspace, delete, tab, escape, enter, arrow keys, and numbers
    const allowedKeys = [
      'Backspace', 'Delete', 'Tab', 'Escape', 'Enter',
      'ArrowLeft', 'ArrowRight', 'ArrowUp', 'ArrowDown'
    ];
    
    // Block non-numeric keys (except allowed ones)
    if (!allowedKeys.includes(event.key) && !/[0-9]/.test(event.key)) {
      event.preventDefault();
      return;
    }

    // Handle backspace
    if (event.key === 'Backspace' && !input.value && index > 0) {
      const prevInput = document.getElementById(`otp-${index - 1}`) as HTMLInputElement;
      if (prevInput) {
        prevInput.focus();
      }
    }

    // Handle arrow keys
    if (event.key === 'ArrowLeft' && index > 0) {
      const prevInput = document.getElementById(`otp-${index - 1}`) as HTMLInputElement;
      if (prevInput) {
        prevInput.focus();
      }
    }

    if (event.key === 'ArrowRight' && index < 5) {
      const nextInput = document.getElementById(`otp-${index + 1}`) as HTMLInputElement;
      if (nextInput) {
        nextInput.focus();
      }
    }
  }

  onOtpPaste(event: ClipboardEvent): void {
    event.preventDefault();
    const pastedData = event.clipboardData?.getData('text').trim() || '';
    const digits = pastedData.replace(/\D/g, '').slice(0, 6);

    digits.split('').forEach((digit, index) => {
      if (index < 6) {
        this.getOtpControl(index).setValue(digit);
        const input = document.getElementById(`otp-${index}`) as HTMLInputElement;
        if (input) {
          input.value = digit;
        }
      }
    });

    // Focus the last filled input or next empty one
    const nextEmptyIndex = Math.min(digits.length, 5);
    const nextInput = document.getElementById(`otp-${nextEmptyIndex}`) as HTMLInputElement;
    if (nextInput) {
      nextInput.focus();
    }
  }

  async onVerifyOtp(): Promise<void> {
    if (this.otpForm.valid && this.registerForm.valid) {
      const otpValue = this.otpArray.controls.map(control => control.value).join('');
      const formValue = this.registerForm.value;
      const mobileNumber = formValue.mobileNumber;

      try {
        // TODO: Call API to verify OTP and complete registration
        console.log('Verifying OTP and registering user:', {
          role: this.selectedRole(),
          otp: otpValue,
          ...formValue
        });

        // Mock registration - in real implementation:
        // 1. Verify OTP with API
        // 2. Complete registration
        // 3. Receive JWT token and user role from API
        // 4. Store token and user data
        // 5. Navigate based on role and returnUrl

        // TODO: Call actual API to verify OTP and complete registration
        // For now, using mock implementation similar to login
        const userRole: 'User' | 'Vendor' = this.selectedRole() === 'Vendor' ? 'Vendor' : 'User';
        
        // Mock API response - in real implementation, this comes from the API
        const mockUser = {
          id: 'user-' + Date.now(),
          email: formValue.email,
          mobile: mobileNumber,
          name: formValue.fullName, // User interface requires 'name' property
          role: userRole
        };
        
        const mockToken = 'mock-jwt-token-' + Date.now();
        
        // Store token and user data using AuthService
        // Note: login() method will auto-navigate, but we'll override if needed
        this.authService.login(mockToken, mockUser);

        // Handle navigation: returnUrl takes priority, then role-based
        if (this.returnUrl && this.returnUrl !== '/') {
          // If returnUrl exists, use it (overrides the navigation from login())
          this.router.navigateByUrl(this.returnUrl);
        } else {
          // No returnUrl: role-based redirection
          if (userRole === 'Vendor') {
            // login() already navigated to vendor dashboard, but ensure it
            this.router.navigate(['/vendor/dashboard']);
          } else {
            // User role: redirect to login page (override the landing page navigation from login())
            this.router.navigate(['/login']);
          }
        }
      } catch (error) {
        console.error('Registration failed:', error);
        // TODO: Show error message to user
        this.otpForm.setErrors({ invalidOtp: true });
      }
    } else {
      this.otpForm.markAllAsTouched();
    }
  }
}

