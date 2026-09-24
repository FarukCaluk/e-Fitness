import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ProductsService } from '../../../core/services/products.service';
import { OrdersService } from '../../../core/services/orders.service';
import { Product } from '../../../core/models/product.model';
import { ProductCategory, PaymentMethod } from '../../../core/models/enums.model';

interface CartLine {
  product: Product;
  quantity: number;
}

@Component({
  selector: 'app-client-shop',
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss'
})
export class ShopComponent implements OnInit {
  products: Product[] = [];
  totalCount = 0;
  pageNumber = 1;
  pageSize = 12;
  searchTerm = '';
  categoryFilter: ProductCategory | '' = '';
  isLoading = false;
  isCheckingOut = false;

  cart: CartLine[] = [];
  showCart = false;

  readonly categories = Object.values(ProductCategory);

  constructor(
    private readonly productsService: ProductsService,
    private readonly ordersService: OrdersService,
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
        category: this.categoryFilter || undefined,
        isActive: true
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

  addToCart(product: Product): void {
    const existing = this.cart.find((line) => line.product.id === product.id);

    if (existing) {
      if (existing.quantity < product.stockQuantity) {
        existing.quantity += 1;
      }
    } else {
      this.cart.push({ product, quantity: 1 });
    }

    this.showCart = true;
  }

  changeQuantity(line: CartLine, delta: number): void {
    line.quantity = Math.max(1, Math.min(line.product.stockQuantity, line.quantity + delta));
  }

  removeFromCart(line: CartLine): void {
    this.cart = this.cart.filter((l) => l !== line);
  }

  get cartTotal(): number {
    return this.cart.reduce((sum, line) => sum + line.product.price * line.quantity, 0);
  }

  get cartCount(): number {
    return this.cart.reduce((sum, line) => sum + line.quantity, 0);
  }

  checkout(): void {
    if (this.cart.length === 0) {
      return;
    }

    this.isCheckingOut = true;

    this.ordersService
      .createOrder({
        items: this.cart.map((line) => ({ productId: line.product.id, quantity: line.quantity })),
        shippingAddress: null,
        paymentMethod: PaymentMethod.CreditCard
      })
      .subscribe({
        next: () => {
          this.isCheckingOut = false;
          this.cart = [];
          this.showCart = false;
          this.snackBar.open('Order placed successfully.', 'Close', { duration: 3000 });
          this.load();
        },
        error: () => (this.isCheckingOut = false)
      });
  }
}
