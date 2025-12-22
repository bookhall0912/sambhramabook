import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface StepperStep {
  number: number;
  label: string;
}

@Component({
  selector: 'app-stepper',
  imports: [CommonModule],
  templateUrl: './stepper.component.html',
  styleUrl: './stepper.component.scss'
})
export class StepperComponent {
  currentStep = input.required<number>();
  steps = input.required<StepperStep[]>();
  stepClick = output<number>();

  onStepClick(stepNumber: number): void {
    if (stepNumber <= this.currentStep()) {
      this.stepClick.emit(stepNumber);
    }
  }

  isStepActive(stepNumber: number): boolean {
    return stepNumber === this.currentStep();
  }

  isStepCompleted(stepNumber: number): boolean {
    return stepNumber < this.currentStep();
  }
}

