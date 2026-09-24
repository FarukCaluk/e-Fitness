import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MembershipPlansService } from '../../../core/services/membership-plans.service';
import { MembershipPlan } from '../../../core/models/membership-plan.model';
import { PlanFormDialogComponent } from './plan-form-dialog/plan-form-dialog.component';

@Component({
  selector: 'app-plans',
  templateUrl: './plans.component.html',
  styleUrl: './plans.component.scss'
})
export class PlansComponent implements OnInit {
  plans: MembershipPlan[] = [];
  isLoading = false;

  constructor(
    private readonly plansService: MembershipPlansService,
    private readonly dialog: MatDialog,
    private readonly snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.isLoading = true;
    this.plansService.getPlans({ pageNumber: 1, pageSize: 50 }).subscribe((result) => {
      this.plans = result.items;
      this.isLoading = false;
    });
  }

  create(): void {
    const dialogRef = this.dialog.open(PlanFormDialogComponent, { width: '480px', data: null });
    dialogRef.afterClosed().subscribe((saved) => {
      if (saved) {
        this.load();
      }
    });
  }

  edit(plan: MembershipPlan): void {
    const dialogRef = this.dialog.open(PlanFormDialogComponent, { width: '480px', data: { plan } });
    dialogRef.afterClosed().subscribe((saved) => {
      if (saved) {
        this.load();
      }
    });
  }

  remove(plan: MembershipPlan): void {
    this.plansService.deletePlan(plan.id).subscribe(() => {
      this.snackBar.open(`${plan.name} deactivated.`, 'Close', { duration: 3000 });
      this.load();
    });
  }
}
