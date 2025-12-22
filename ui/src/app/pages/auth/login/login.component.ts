import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, FormArray, FormControl } from '@angular/forms';
import { Router, RouterLink, ActivatedRoute } from '@angular/router';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-login',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  otpSent = signal<boolean>(false);
  otpForm: FormGroup;
  returnUrl: string = '/';
  isVendorFlow = signal<boolean>(false);

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private authService: AuthService
  ) {
    this.loginForm = this.fb.group({
      mobileOrEmail: ['', [Validators.required]]
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
    
    // Get returnUrl and vendor flag from query params
    this.route.queryParams.subscribe(params => {
      this.returnUrl = params['returnUrl'] || '/';
      this.isVendorFlow.set(params['vendor'] === 'true');
    });
  }

  get otpArray(): FormArray {
    return this.otpForm.get('otp') as FormArray;
  }

  getOtpControl(index: number): FormControl {
    return this.otpArray.at(index) as FormControl;
  }

  onMobileInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    let value = input.value;

    // Remove any non-digit characters (allow only digits and spaces for formatting)
    value = value.replace(/[^\d\s]/g, '');

    // Update input value
    if (input.value !== value) {
      input.value = value;
      this.loginForm.get('mobileOrEmail')?.setValue(value, { emitEvent: false });
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

  onSendOtp(): void {
    if (this.loginForm.valid) {
      const mobileOrEmail = this.loginForm.get('mobileOrEmail')?.value;
      // TODO: Implement OTP sending logic
      console.log('Sending OTP to:', mobileOrEmail);
      this.otpSent.set(true);
      // Focus first OTP input after a short delay
      setTimeout(() => {
        const firstInput = document.getElementById('otp-0') as HTMLInputElement;
        if (firstInput) {
          firstInput.focus();
        }
      }, 100);
    } else {
      this.loginForm.markAllAsTouched();
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
    if (this.otpForm.valid) {
      const otpValue = this.otpArray.controls.map(control => control.value).join('');
      const mobileOrEmail = this.loginForm.get('mobileOrEmail')?.value;

      try {
        // Verify OTP with API (mock for now)
        const { token, user } = await this.authService.verifyOtp(mobileOrEmail, otpValue);
        
        // Login user
        this.authService.login(token, user);
        
        // Handle navigation: prioritize role-based routing
        if (user.role === 'Admin') {
          this.router.navigate(['/admin/dashboard']);
        } else if (user.role === 'Vendor') {
          // Check if vendor profile is complete
          const userData = localStorage.getItem('user_data');
          if (userData) {
            const parsedUser = JSON.parse(userData);
            if (!parsedUser.vendorProfileComplete) {
              // Redirect to onboarding if profile not complete
              this.router.navigate(['/vendor/onboarding']);
            } else {
              // Profile complete, go to dashboard
              this.router.navigate(['/vendor/dashboard']);
            }
          } else {
            // No user data, go to onboarding
            this.router.navigate(['/vendor/onboarding']);
          }
        } else {
          // For regular users, use returnUrl if provided, otherwise go to home
          if (this.returnUrl && this.returnUrl !== '/') {
            this.router.navigateByUrl(this.returnUrl);
          } else {
            this.router.navigate(['/']);
          }
        }
      } catch (error) {
        console.error('OTP verification failed:', error);
        // TODO: Show error message to user
        this.otpForm.setErrors({ invalidOtp: true });
      }
    } else {
      this.otpForm.markAllAsTouched();
    }
  }
}

