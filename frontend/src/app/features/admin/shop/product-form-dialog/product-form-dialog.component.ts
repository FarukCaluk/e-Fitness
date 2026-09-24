import { Component, Inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ProductsService } from '../../../../core/services/products.service';
import { Product } from '../../../../core/models/product.model';
import { ProductCategory } from '../../../../core/models/enums.model';

export interface ProductFormDialogData {
  product: Product;
}

@Component({
  selector: 'app-product-form-dialog',
  templateUrl: './product-form-dialog.component.html',
  styleUrl: './product-form-dialog.component.scss'
})
export class ProductFormDialogComponent {
  readonly isEditMode: boolean;
  readonly categories = Object.values(ProductCategory);
  readonly form;

  isSaving = false;

  constructor(
    private readonly fb: FormBuilder,
    private readonly productsService: ProductsService,
    private readonly dialogRef: MatDialogRef<ProductFormDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ProductFormDialogData | null
  ) {
    this.isEditMode = !!this.data;

    this.form = this.fb.group({
      name: [this.data?.product.name ?? '', Validators.required],
      description: [this.data?.product.description ?? ''],
      price: [this.data?.product.price ?? 0, [Validators.required, Validators.min(0)]],
      category: [this.data?.product.category ?? ProductCategory.Supplements, Validators.required],
      stockQuantity: [this.data?.product.stockQuantity ?? 0, [Validators.required, Validators.min(0)]],
      imageUrl: [this.data?.product.imageUrl ?? ''],
      isActive: [this.data?.product.isActive ?? true]
    });
  }

  save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isSaving = true;
    const raw = this.form.getRawValue();
    const payload = {
      name: raw.name!,
      description: raw.description || null,
      price: raw.price!,
      category: raw.category!,
      stockQuantity: raw.stockQuantity!,
      imageUrl: raw.imageUrl || null
    };

    if (this.isEditMode) {
      this.productsService.updateProduct(this.data!.product.id, { ...payload, isActive: raw.isActive! }).subscribe({
        next: () => this.dialogRef.close(true),
        error: () => (this.isSaving = false)
      });
      return;
    }

    this.productsService.createProduct(payload).subscribe({
      next: () => this.dialogRef.close(true),
      error: () => (this.isSaving = false)
    });
  }

  close(): void {
    this.dialogRef.close(false);
  }
}
