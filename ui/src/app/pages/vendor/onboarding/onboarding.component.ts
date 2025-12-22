import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { HeaderComponent } from '../../../components/header/header.component';
import { FooterComponent } from '../../../components/footer/footer.component';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-vendor-onboarding',
  imports: [CommonModule, ReactiveFormsModule, HeaderComponent, FooterComponent],
  templateUrl: './onboarding.component.html',
  styleUrl: './onboarding.component.scss'
})
export class VendorOnboardingComponent implements OnInit {
  onboardingForm: FormGroup;
  currentStep = signal<number>(1);
  totalSteps = 3;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private authService: AuthService
  ) {
    this.onboardingForm = this.fb.group({
      // Step 1: Business Information
      businessName: ['', [Validators.required]],
      businessType: ['Hall Owner', [Validators.required]],
      businessEmail: ['', [Validators.required, Validators.email]],
      businessPhone: ['', [Validators.required]],
      
      // Step 2: Location
      address: ['', [Validators.required]],
      city: ['', [Validators.required]],
      state: ['', [Validators.required]],
      pincode: ['', [Validators.required]],
      
      // Step 3: Additional Details
      gstNumber: [''],
      panNumber: [''],
      bankAccountNumber: [''],
      ifscCode: ['']
    });
  }

  ngOnInit(): void {
    window.scrollTo(0, 0);
    
    // Pre-fill with user data if available
    const user = this.authService.currentUser$();
    if (user) {
      this.onboardingForm.patchValue({
        businessEmail: user.email || '',
        businessPhone: user.mobile || ''
      });
    }
  }

  nextStep(): void {
    if (this.isCurrentStepValid()) {
      if (this.currentStep() < this.totalSteps) {
        this.currentStep.update(step => step + 1);
      } else {
        this.completeOnboarding();
      }
    } else {
      this.markCurrentStepFieldsAsTouched();
    }
  }

  previousStep(): void {
    if (this.currentStep() > 1) {
      this.currentStep.update(step => step - 1);
    }
  }

  isCurrentStepValid(): boolean {
    const step = this.currentStep();
    if (step === 1) {
      return !!(this.onboardingForm.get('businessName')?.valid &&
             this.onboardingForm.get('businessType')?.valid &&
             this.onboardingForm.get('businessEmail')?.valid &&
             this.onboardingForm.get('businessPhone')?.valid);
    } else if (step === 2) {
      return !!(this.onboardingForm.get('address')?.valid &&
             this.onboardingForm.get('city')?.valid &&
             this.onboardingForm.get('state')?.valid &&
             this.onboardingForm.get('pincode')?.valid);
    }
    return true; // Step 3 is optional
  }

  markCurrentStepFieldsAsTouched(): void {
    const step = this.currentStep();
    if (step === 1) {
      ['businessName', 'businessType', 'businessEmail', 'businessPhone'].forEach(field => {
        this.onboardingForm.get(field)?.markAsTouched();
      });
    } else if (step === 2) {
      ['address', 'city', 'state', 'pincode'].forEach(field => {
        this.onboardingForm.get(field)?.markAsTouched();
      });
    }
  }

  completeOnboarding(): void {
    if (this.onboardingForm.valid) {
      // TODO: Save vendor profile to API
      const formData = this.onboardingForm.value;
      console.log('Completing onboarding:', formData);
      
      // Mark vendor profile as complete in localStorage
      const user = this.authService.currentUser$();
      if (user) {
        const updatedUser = {
          ...user,
          vendorProfileComplete: true
        };
        localStorage.setItem('user_data', JSON.stringify(updatedUser));
      }
      
      // Redirect to dashboard
      this.router.navigate(['/vendor/dashboard']);
    }
  }

  skipForNow(): void {
    // Allow skipping but mark as incomplete
    this.router.navigate(['/vendor/dashboard']);
  }
}

