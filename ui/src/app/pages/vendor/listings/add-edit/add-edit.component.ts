import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterLinkActive, ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../../services/auth.service';

export interface Amenity {
  id: string;
  name: string;
  selected: boolean;
}

@Component({
  selector: 'app-vendor-add-edit-listing',
  imports: [CommonModule, RouterLink, RouterLinkActive, FormsModule],
  templateUrl: './add-edit.component.html',
  styleUrl: './add-edit.component.scss'
})
export class VendorAddEditListingComponent implements OnInit {
  isEditMode = signal(false);
  currentStep = signal<number>(1);
  progress = signal<number>(65);

  // Form data
  hallName = signal<string>('');
  hallType = signal<string>('Convention Hall');
  yearEstablished = signal<string>('');
  description = signal<string>('');

  addressLine1 = signal<string>('');
  area = signal<string>('');
  city = signal<string>('Bangalore');
  pincode = signal<string>('');

  floatingCapacity = signal<string>('');
  diningCapacity = signal<string>('');

  pricePerDay = signal<string>('');
  advanceAmount = signal<string>('');
  cancellationPolicy = signal<string>('');

  amenities = signal<Amenity[]>([
    { id: 'ac', name: 'Air Conditioning', selected: true },
    { id: 'parking', name: 'Ample Parking', selected: false },
    { id: 'power', name: 'Power Backup', selected: true },
    { id: 'changing', name: 'Changing Rooms', selected: false },
    { id: 'kitchen', name: 'Catering Kitchen', selected: false },
    { id: 'lift', name: 'Lift Access', selected: false }
  ]);

  uploadedImages = signal<string[]>([]);

  steps = [
    { number: 1, label: 'Basic Info' },
    { number: 2, label: 'Capacity' },
    { number: 3, label: 'Pricing' },
    { number: 4, label: 'Availability' },
    { number: 5, label: 'Photos' },
    { number: 6, label: 'Review' }
  ];

  pendingBookingsCount = signal<number>(1);

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    window.scrollTo(0, 0);
    // Check if editing
    const id = this.route.snapshot.paramMap.get('id');
    if (id && this.route.snapshot.url.some(segment => segment.path === 'edit')) {
      this.isEditMode.set(true);
      // TODO: Load existing listing data
      this.loadListingData(id);
    }
  }

  private loadListingData(id: string): void {
    // TODO: Load from API
    // For now, set some default values
    this.hallName.set('Royal Grand Hall');
    this.yearEstablished.set('2015');
    this.description.set('A grand convention hall perfect for weddings and events.');
    this.addressLine1.set('123 Main Street');
    this.area.set('Jayanagar');
    this.pincode.set('560041');
    this.floatingCapacity.set('1000');
    this.diningCapacity.set('400');
    this.pricePerDay.set('150000');
    this.advanceAmount.set('50');
    this.cancellationPolicy.set('No refund if cancelled within 30 days');
  }

  toggleAmenity(amenityId: string): void {
    this.amenities.update(amenities =>
      amenities.map(a => a.id === amenityId ? { ...a, selected: !a.selected } : a)
    );
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const files = Array.from(input.files);
      files.forEach(file => {
        const reader = new FileReader();
        reader.onload = (e: any) => {
          this.uploadedImages.update(images => [...images, e.target.result]);
        };
        reader.readAsDataURL(file);
      });
    }
  }

  removeImage(index: number): void {
    this.uploadedImages.update(images => images.filter((_, i) => i !== index));
  }

  onSaveDraft(): void {
    // TODO: Save as draft via API
    console.log('Saving as draft...');
    alert('Draft saved successfully!');
  }

  onNextStep(): void {
    if (this.currentStep() < this.steps.length) {
      this.currentStep.update(step => step + 1);
      this.updateProgress();
    }
  }

  onPreviousStep(): void {
    if (this.currentStep() > 1) {
      this.currentStep.update(step => step - 1);
      this.updateProgress();
    }
  }

  goToStep(step: number): void {
    this.currentStep.set(step);
    this.updateProgress();
  }

  private updateProgress(): void {
    const progress = Math.round((this.currentStep() / this.steps.length) * 100);
    this.progress.set(progress);
  }

  onSubmit(): void {
    // TODO: Submit form via API
    console.log('Submitting listing...');
    alert('Listing submitted successfully!');
    this.router.navigate(['/vendor/dashboard/listings']);
  }

  onLogout(): void {
    this.authService.logout();
  }
}

