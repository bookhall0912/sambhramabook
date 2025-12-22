import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { DatePickerComponent } from '../date-picker/date-picker.component';

@Component({
  selector: 'app-venue-search-form',
  imports: [CommonModule, ReactiveFormsModule, DatePickerComponent],
  templateUrl: './venue-search-form.component.html',
  styleUrl: './venue-search-form.component.scss'
})
export class VenueSearchFormComponent implements OnInit {
  searchForm: FormGroup;
  readonly today = new Date();
  readonly minDate = new Date(); // Today is minimum date
  
  // Mock suggestions for autocomplete
  suggestions: string[] = [];
  showSuggestions = false;
  
  // Common locations for suggestions
  private readonly commonLocations = [
    'Bangalore',
    'Mumbai',
    'Delhi',
    'Hyderabad',
    'Chennai',
    'Pune',
    'Kolkata',
    'Ahmedabad'
  ];
  
  constructor(
    private fb: FormBuilder,
    private router: Router
  ) {
    // Format today's date as YYYY-MM-DD for the form
    const todayStr = this.formatDate(this.today);
    
    this.searchForm = this.fb.group({
      location: ['', Validators.required],
      date: [todayStr, Validators.required],
      days: [1, [Validators.required, Validators.min(1)]],
      guests: [500, [Validators.required, Validators.min(1)]]
    });
  }

  ngOnInit(): void {
    // Watch location input changes for simple autocomplete
    this.searchForm.get('location')?.valueChanges.subscribe((value) => {
      if (value && value.length > 0) {
        this.suggestions = this.commonLocations.filter(loc => 
          loc.toLowerCase().includes(value.toLowerCase())
        );
        this.showSuggestions = this.suggestions.length > 0;
      } else {
        this.suggestions = [];
        this.showSuggestions = false;
      }
    });
  }

  private formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  onSearch(): void {
    if (this.searchForm.valid) {
      const formValue = this.searchForm.value;
      const location = formValue.location;

      // Navigate to halls page with search query params
      // Using default coordinates (Bangalore) for mock data
      this.router.navigate(['/halls'], {
        queryParams: {
          location: location,
          latitude: 12.9716, // Default to Bangalore
          longitude: 77.5946,
          date: formValue.date,
          days: formValue.days,
          guests: formValue.guests
        }
      });
    } else {
      this.searchForm.markAllAsTouched();
    }
  }

  selectSuggestion(suggestion: string): void {
    this.searchForm.patchValue({ location: suggestion });
    this.suggestions = [];
    this.showSuggestions = false;
  }

  onLocationBlur(): void {
    // Hide suggestions after a short delay to allow click events
    setTimeout(() => {
      this.showSuggestions = false;
    }, 200);
  }

  onLocationFocus(): void {
    const locationValue = this.searchForm.get('location')?.value;
    if (locationValue && locationValue.length > 0) {
      this.suggestions = this.commonLocations.filter(loc => 
        loc.toLowerCase().includes(locationValue.toLowerCase())
      );
      this.showSuggestions = this.suggestions.length > 0;
    }
  }
}
