import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { LayoutModule } from '../../layout/layout.module';
import { SharedModule } from '../../shared/shared.module';

import { AdminRoutingModule } from './admin-routing.module';
import { AnnouncementsComponent } from './announcements/announcements.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { FinanceComponent } from './finance/finance.component';
import { MemberEditDialogComponent } from './members/member-edit-dialog/member-edit-dialog.component';
import { MembersComponent } from './members/members.component';
import { PlanFormDialogComponent } from './plans/plan-form-dialog/plan-form-dialog.component';
import { PlansComponent } from './plans/plans.component';
import { ProductFormDialogComponent } from './shop/product-form-dialog/product-form-dialog.component';
import { ShopComponent } from './shop/shop.component';
import { TrainerFormDialogComponent } from './trainers/trainer-form-dialog/trainer-form-dialog.component';
import { TrainersComponent } from './trainers/trainers.component';

@NgModule({
  declarations: [
    DashboardComponent,
    MembersComponent,
    MemberEditDialogComponent,
    TrainersComponent,
    TrainerFormDialogComponent,
    PlansComponent,
    PlanFormDialogComponent,
    ShopComponent,
    ProductFormDialogComponent,
    FinanceComponent,
    AnnouncementsComponent
  ],
  imports: [CommonModule, SharedModule, LayoutModule, AdminRoutingModule]
})
export class AdminModule {}
