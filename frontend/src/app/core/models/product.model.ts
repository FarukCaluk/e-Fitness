import { ProductCategory } from './enums.model';

export interface Product {
  id: number;
  name: string;
  description?: string | null;
  price: number;
  category: ProductCategory;
  stockQuantity: number;
  imageUrl?: string | null;
  isActive: boolean;
}

export interface CreateProductRequest {
  name: string;
  description?: string | null;
  price: number;
  category: ProductCategory;
  stockQuantity: number;
  imageUrl?: string | null;
}

export interface UpdateProductRequest extends CreateProductRequest {
  isActive: boolean;
}
