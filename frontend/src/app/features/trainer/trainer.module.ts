import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { LayoutModule } from '../../layout/layout.module';
import { ChatModule } from '../../shared/chat/chat.module';
import { SharedModule } from '../../shared/shared.module';

import { ClientsComponent } from './clients/clients.component';
import { ScheduleSessionDialogComponent } from './clients/schedule-session-dialog/schedule-session-dialog.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { TrainerRoutingModule } from './trainer-routing.module';
import { WorkoutBuilderComponent } from './workout-builder/workout-builder.component';

@NgModule({
  declarations: [DashboardComponent, ClientsComponent, ScheduleSessionDialogComponent, WorkoutBuilderComponent],
  imports: [CommonModule, SharedModule, LayoutModule, ChatModule, TrainerRoutingModule]
})
export class TrainerModule {}
