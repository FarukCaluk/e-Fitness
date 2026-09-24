import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { PaymentsService } from '../../../core/services/payments.service';
import { Payment } from '../../../core/models/payment.model';
import { PaymentStatus } from '../../../core/models/enums.model';

@Component({
  selector: 'app-finance',
  templateUrl: './finance.component.html',
  styleUrl: './finance.component.scss'
})
export class FinanceComponent implements OnInit {
  payments: Payment[] = [];
  totalCount = 0;
  pageNumber = 1;
  pageSize = 10;
  statusFilter: PaymentStatus | '' = '';
  isLoading = false;

  readonly statuses = Object.values(PaymentStatus);
  readonly displayedColumns = ['member', 'amount', 'method', 'purpose', 'status', 'date'];

  constructor(private readonly paymentsService: PaymentsService) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.isLoading = true;
    this.paymentsService
      .getPayments({ pageNumber: this.pageNumber, pageSize: this.pageSize, status: this.statusFilter || undefined })
      .subscribe((result) => {
        this.payments = result.items;
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
}
