import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MembershipPlansService } from '../../../core/services/membership-plans.service';
import { MembershipsService } from '../../../core/services/memberships.service';
import { MembershipPlan } from '../../../core/models/membership-plan.model';
import { Membership } from '../../../core/models/membership.model';
import { MembershipStatus, PaymentMethod } from '../../../core/models/enums.model';

@Component({
  selector: 'app-client-plans',
  templateUrl: './plans.component.html',
  styleUrl: './plans.component.scss'
})
export class PlansComponent implements OnInit {
  plans: MembershipPlan[] = [];
  currentMembership: Membership | null = null;
  isLoading = true;
  processingPlanId: number | null = null;

  constructor(
    private readonly plansService: MembershipPlansService,
    private readonly membershipsService: MembershipsService,
    private readonly snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.isLoading = true;
    this.plansService.getPlans({ pageNumber: 1, pageSize: 50, isActive: true }).subscribe((result) => {
      this.plans = result.items;
      this.isLoading = false;
    });

    this.membershipsService.getMemberships({ pageNumber: 1, pageSize: 1, status: MembershipStatus.Active }).subscribe((result) => {
      this.currentMembership = result.items[0] ?? null;
    });
  }

  isCurrentPlan(plan: MembershipPlan): boolean {
    return this.currentMembership?.membershipPlanId === plan.id;
  }

  subscribe(plan: MembershipPlan): void {
    this.processingPlanId = plan.id;

    this.membershipsService
      .subscribe({ membershipPlanId: plan.id, paymentMethod: PaymentMethod.CreditCard, autoRenew: true })
      .subscribe({
        next: () => {
          this.processingPlanId = null;
          this.snackBar.open(`Subscribed to ${plan.name}.`, 'Close', { duration: 3000 });
          this.load();
        },
        error: () => (this.processingPlanId = null)
      });
  }

  cancelMembership(): void {
    if (!this.currentMembership) {
      return;
    }

    this.membershipsService.cancel(this.currentMembership.id).subscribe(() => {
      this.snackBar.open('Membership cancelled.', 'Close', { duration: 3000 });
      this.load();
    });
  }
}
