import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';

import { DashboardComponent } from './dashboard/dashboard.component';
import { TrainerRoutingModule } from './trainer-routing.module';

@NgModule({
  declarations: [DashboardComponent],
  imports: [CommonModule, SharedModule, TrainerRoutingModule]
})
export class TrainerModule {}
