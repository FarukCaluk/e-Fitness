import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ProductsService } from '../../../core/services/products.service';
import { Product } from '../../../core/models/product.model';
import { ProductCategory } from '../../../core/models/enums.model';
import { ProductFormDialogComponent } from './product-form-dialog/product-form-dialog.component';

@Component({
  selector: 'app-shop-admin',
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss'
})
export class ShopComponent implements OnInit {
  products: Product[] = [];
  totalCount = 0;
  pageNumber = 1;
  pageSize = 10;
  searchTerm = '';
  categoryFilter: ProductCategory | '' = '';
  isLoading = false;

  readonly categories = Object.values(ProductCategory);
  readonly displayedColumns = ['name', 'category', 'price', 'stock', 'status', 'actions'];

  constructor(
    private readonly productsService: ProductsService,
    private readonly dialog: MatDialog,
    private readonly snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.isLoading = true;
    this.productsService
      .getProducts({
        pageNumber: this.pageNumber,
        pageSize: this.pageSize,
        searchTerm: this.searchTerm || undefined,
        category: this.categoryFilter || undefined
      })
      .subscribe((result) => {
        this.products = result.items;
        this.totalCount = result.totalCount;
        this.isLoading = false;
      });
  }

  onFilterChange(): void {
    this.pageNumber = 1;
    this.load();
  }

  onPageChange(event: PageEvent): void {
    this.pageNumber = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.load();
  }

  create(): void {
    const dialogRef = this.dialog.open(ProductFormDialogComponent, { width: '480px', data: null });
    dialogRef.afterClosed().subscribe((saved) => saved && this.load());
  }

  edit(product: Product): void {
    const dialogRef = this.dialog.open(ProductFormDialogComponent, { width: '480px', data: { product } });
    dialogRef.afterClosed().subscribe((saved) => saved && this.load());
  }

  remove(product: Product): void {
    this.productsService.deleteProduct(product.id).subscribe(() => {
      this.snackBar.open(`${product.name} deactivated.`, 'Close', { duration: 3000 });
      this.load();
    });
  }
}
