import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { LayoutModule } from '../../layout/layout.module';
import { ChatModule } from '../../shared/chat/chat.module';
import { SharedModule } from '../../shared/shared.module';

import { ClientRoutingModule } from './client-routing.module';
import { DashboardComponent } from './dashboard/dashboard.component';
import { MyWorkoutComponent } from './my-workout/my-workout.component';
import { PlansComponent } from './plans/plans.component';
import { ProgressComponent } from './progress/progress.component';
import { ShopComponent } from './shop/shop.component';

@NgModule({
  declarations: [DashboardComponent, MyWorkoutComponent, ProgressComponent, ShopComponent, PlansComponent],
  imports: [CommonModule, SharedModule, LayoutModule, ChatModule, ClientRoutingModule]
})
export class ClientModule {}
