import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-profile',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink,
    RouterLinkActive
  ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent implements OnInit {
  profileForm: FormGroup;
  isEditing = signal<boolean>(false);

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private authService: AuthService
  ) {
    const user = this.authService.currentUser$();
    
    this.profileForm = this.fb.group({
      fullName: [user?.name || '', [Validators.required]],
      email: [user?.email || '', [Validators.required, Validators.email]],
      mobile: [user?.mobile || '', [Validators.required]],
      address: [''],
      city: [''],
      state: [''],
      pincode: ['']
    });
  }

  ngOnInit(): void {
    window.scrollTo(0, 0);
    // TODO: Load user profile data from API
  }

  onEdit(): void {
    this.isEditing.set(true);
  }

  onCancel(): void {
    this.isEditing.set(false);
    // Reset form to original values
    const user = this.authService.currentUser$();
    this.profileForm.patchValue({
      fullName: user?.name || '',
      email: user?.email || '',
      mobile: user?.mobile || ''
    });
  }

  onSave(): void {
    if (this.profileForm.valid) {
      // TODO: Call API to save profile
      console.log('Saving profile:', this.profileForm.value);
      this.isEditing.set(false);
      // TODO: Update auth service with new user data
    } else {
      this.profileForm.markAllAsTouched();
    }
  }

  onChangePassword(): void {
    // TODO: Open change password modal or navigate to change password page
    console.log('Change password');
  }

  onLogout(): void {
    this.authService.logout();
  }
}

