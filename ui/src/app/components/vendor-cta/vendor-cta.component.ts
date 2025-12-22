import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-vendor-cta',
  imports: [CommonModule],
  templateUrl: './vendor-cta.component.html',
  styleUrl: './vendor-cta.component.scss'
})
export class VendorCtaComponent {
  constructor(private router: Router) {}

  onGetStarted(): void {
    this.router.navigate(['/vendors']);
  }
}

