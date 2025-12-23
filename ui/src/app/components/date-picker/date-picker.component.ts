import { Component, input, output, OnInit, OnDestroy, ElementRef, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ControlValueAccessor, NG_VALUE_ACCESSOR, FormsModule } from '@angular/forms';
import flatpickr from 'flatpickr';
// flatpickr-year-select-plugin is a UMD module without proper default export
// The fallback handles both default and namespace exports
import * as yearSelectPluginModule from 'flatpickr-year-select-plugin';
const yearSelectPlugin = (yearSelectPluginModule as any).default || yearSelectPluginModule;

@Component({
  selector: 'app-date-picker',
  imports: [CommonModule, FormsModule],
  templateUrl: './date-picker.component.html',
  styleUrl: './date-picker.component.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: DatePickerComponent,
      multi: true
    }
  ]
})
export class DatePickerComponent implements OnInit, OnDestroy, ControlValueAccessor {
  @ViewChild('dateInput', { static: true }) dateInput!: ElementRef<HTMLInputElement>;

  placeholder = input<string>('Select date');
  minDate = input<Date | undefined>(undefined);
  maxDate = input<Date | undefined>(undefined);
  disabled = input<boolean>(false);
  dateChanged = output<Date | null>();

  private flatpickrInstance: flatpickr.Instance | null = null;
  private onChange = (value: string | null) => {};
  private onTouched = () => {};

  ngOnInit(): void {
    // Initialize after view is ready
    setTimeout(() => {
      this.initializeFlatpickr();
    }, 0);
  }

  ngOnDestroy(): void {
    if (this.flatpickrInstance) {
      this.flatpickrInstance.destroy();
    }
  }

  private initializeFlatpickr(): void {
    const today = new Date();
    const currentYear = today.getFullYear();
    const initialValue = this.dateInput.nativeElement.value || this.formatDate(today);
    
    const options: flatpickr.Options.Options = {
      dateFormat: 'Y-m-d',
      defaultDate: initialValue ? new Date(initialValue) : today,
      minDate: this.minDate() || today,
      maxDate: this.maxDate(),
      disableMobile: false,
      monthSelectorType: 'dropdown',
      plugins: [
        yearSelectPlugin({
          start: 0, // Start from current year
          end: 10   // Include next 10 years
        })
      ],
      locale: {
        firstDayOfWeek: 1,
        weekdays: {
          shorthand: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
          longhand: ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday']
        },
        months: {
          shorthand: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
          longhand: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December']
        }
      },
      onChange: (selectedDates, dateStr) => {
        const date = selectedDates.length > 0 ? selectedDates[0] : null;
        if (date) {
          const dateStr = this.formatDate(date);
          this.onChange(dateStr);
          this.dateChanged.emit(date);
        } else {
          this.onChange(null);
          this.dateChanged.emit(null);
        }
      },
      onClose: () => {
        this.onTouched();
      }
    };

    this.flatpickrInstance = flatpickr(this.dateInput.nativeElement, options);
    
    // Ensure today's date is set if no value
    if (!this.dateInput.nativeElement.value) {
      this.flatpickrInstance.setDate(today, false);
      this.onChange(this.formatDate(today));
    }

    // After calendar is rendered, ensure year is dropdown and hide plain text
    setTimeout(() => {
      this.ensureYearDropdown();
    }, 100);
  }

  private ensureYearDropdown(): void {
    if (!this.flatpickrInstance) return;

    const calendar = this.flatpickrInstance.calendarContainer;
    if (!calendar) return;

    // Remove year input field completely
    const yearInput = calendar.querySelector('.flatpickr-current-month input[type="number"]');
    if (yearInput && yearInput.parentNode) {
      yearInput.parentNode.removeChild(yearInput);
    }

    // Remove numInputWrapper if it exists
    const numInputWrapper = calendar.querySelector('.flatpickr-current-month .numInputWrapper');
    if (numInputWrapper && numInputWrapper.parentNode) {
      numInputWrapper.parentNode.removeChild(numInputWrapper);
    }

    // Remove any plain text year display
    const yearText = calendar.querySelector('.flatpickr-current-month .cur-year');
    if (yearText && yearText.parentNode) {
      yearText.parentNode.removeChild(yearText);
    }

    // Remove any top-level year selector
    const topYearSelector = calendar.querySelector('.flatpickr-months > select:first-child');
    if (topYearSelector && topYearSelector.parentNode) {
      topYearSelector.parentNode.removeChild(topYearSelector);
    }

    // Ensure year dropdown is visible next to month
    const yearDropdown = calendar.querySelector('.flatpickr-current-month select.flatpickr-yearDropdown-years');
    if (yearDropdown && yearDropdown instanceof HTMLElement) {
      yearDropdown.style.display = 'inline-block';
      yearDropdown.style.verticalAlign = 'middle';
      yearDropdown.style.margin = '0';
    }
    
    // Ensure month dropdown is inline
    const monthDropdown = calendar.querySelector('.flatpickr-current-month select.flatpickr-monthDropdown-months');
    if (monthDropdown && monthDropdown instanceof HTMLElement) {
      monthDropdown.style.display = 'inline-block';
      monthDropdown.style.verticalAlign = 'middle';
      monthDropdown.style.margin = '0';
    }
    
    // Force current month container to be inline
    const currentMonth = calendar.querySelector('.flatpickr-current-month');
    if (currentMonth && currentMonth instanceof HTMLElement) {
      currentMonth.style.display = 'flex';
      currentMonth.style.flexDirection = 'row';
      currentMonth.style.flexWrap = 'nowrap';
      currentMonth.style.whiteSpace = 'nowrap';
      currentMonth.style.alignItems = 'center';
    }
  }

  // ControlValueAccessor implementation
  writeValue(value: Date | string | null): void {
    if (this.flatpickrInstance) {
      if (value) {
        const date = typeof value === 'string' ? new Date(value) : value;
        if (!isNaN(date.getTime())) {
          this.flatpickrInstance.setDate(date, false);
        } else {
          // If invalid date, set to today
          this.flatpickrInstance.setDate(new Date(), false);
        }
      } else {
        // If no value, set to today as default
        this.flatpickrInstance.setDate(new Date(), false);
      }
    } else if (this.dateInput) {
      if (value) {
        const date = typeof value === 'string' ? new Date(value) : value;
        if (!isNaN(date.getTime())) {
          this.dateInput.nativeElement.value = this.formatDate(date);
        } else {
          this.dateInput.nativeElement.value = this.formatDate(new Date());
        }
      } else {
        this.dateInput.nativeElement.value = this.formatDate(new Date());
      }
    }
  }

  registerOnChange(fn: (value: string | null) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    if (this.flatpickrInstance) {
      if (isDisabled) {
        this.flatpickrInstance.input.disabled = true;
        this.flatpickrInstance.close();
      } else {
        this.flatpickrInstance.input.disabled = false;
      }
    } else if (this.dateInput) {
      this.dateInput.nativeElement.disabled = isDisabled;
    }
  }

  private formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }
}

