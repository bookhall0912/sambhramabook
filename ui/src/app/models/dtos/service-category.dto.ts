/**
 * Service Category DTO
 */
export interface ServiceCategoryDto {
  code: string;
  displayName: string;
  description: string | null;
  iconUrl: string | null;
  backgroundImageUrl: string | null;
  themeColor: string | null;
  displayOrder: number;
}

/**
 * Service Categories Response DTO
 */
export interface ServiceCategoriesResponseDto {
  categories: ServiceCategoryDto[];
}

