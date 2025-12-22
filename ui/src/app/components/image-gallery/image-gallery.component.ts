import { Component, input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-image-gallery',
  imports: [CommonModule],
  templateUrl: './image-gallery.component.html',
  styleUrl: './image-gallery.component.scss'
})
export class ImageGalleryComponent implements OnInit {
  images = input.required<string[]>();
  alt = input<string>('Gallery image');
  mainImageIndex = input<number>(0);

  selectedIndex = 0;

  ngOnInit(): void {
    this.selectedIndex = this.mainImageIndex();
  }

  selectImage(imageUrl: string): void {
    const index = this.images().indexOf(imageUrl);
    if (index !== -1) {
      this.selectedIndex = index;
    }
  }

  getMainImage(): string {
    return this.images()[this.selectedIndex] || this.images()[0];
  }

  getThumbnails(): string[] {
    const allImages = this.images();
    return allImages.slice(0, 3).filter((_, index) => index !== this.selectedIndex);
  }
}

