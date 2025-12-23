import { Component, OnInit, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { UserApiService } from '../../../services/user.api';
import { catchError, of } from 'rxjs';

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

  private userApiService = inject(UserApiService);

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
    this.loadUserProfile();
  }

  private loadUserProfile(): void {
    this.userApiService.getUserProfile()
      .pipe(
        catchError(error => {
          console.error('Error loading user profile:', error);
          return of({ success: false, data: null });
        })
      )
      .subscribe(response => {
        if (response.success && response.data) {
          const profile = response.data;
          this.profileForm.patchValue({
            fullName: profile.name,
            email: profile.email || '',
            mobile: profile.mobile || '',
            address: profile.address || '',
            city: profile.city || '',
            state: profile.state || '',
            pincode: profile.pincode || ''
          });
        }
      });
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
      const formValue = this.profileForm.value;
      this.userApiService.updateUserProfile({
        name: formValue.fullName,
        email: formValue.email,
        address: formValue.address,
        city: formValue.city,
        state: formValue.state,
        pincode: formValue.pincode
      })
        .pipe(
          catchError(error => {
            console.error('Error updating profile:', error);
            alert('Failed to update profile. Please try again.');
            return of(null);
          })
        )
        .subscribe(response => {
          if (response) {
            // Update auth service with new user data
            const user = this.authService.currentUser$();
            if (user) {
              const updatedUser = {
                ...user,
                name: formValue.fullName,
                email: formValue.email
              };
              this.authService.login(this.authService.getToken() || '', updatedUser);
            }
            this.isEditing.set(false);
            alert('Profile updated successfully!');
          }
        });
    } else {
      this.profileForm.markAllAsTouched();
    }
  }

  onChangePassword(): void {
    const currentPassword = prompt('Enter current password:');
    if (!currentPassword) return;
    
    const newPassword = prompt('Enter new password:');
    if (!newPassword) return;
    
    const confirmPassword = prompt('Confirm new password:');
    if (newPassword !== confirmPassword) {
      alert('Passwords do not match!');
      return;
    }
    
    this.userApiService.changePassword({ currentPassword, newPassword })
      .pipe(
        catchError(error => {
          console.error('Error changing password:', error);
          alert('Failed to change password. Please try again.');
          return of(null);
        })
      )
      .subscribe(response => {
        if (response) {
          alert('Password changed successfully!');
        }
      });
  }

  onLogout(): void {
    this.authService.logout();
  }
}

