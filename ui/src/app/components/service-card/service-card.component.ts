import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Service } from '../services-section/services-section.component';

@Component({
  selector: 'app-service-card',
  imports: [CommonModule, RouterLink],
  templateUrl: './service-card.component.html',
  styleUrl: './service-card.component.scss'
})
export class ServiceCardComponent {
  service = input.required<Service>();
}
