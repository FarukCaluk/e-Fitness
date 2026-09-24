import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { LayoutModule } from '../../layout/layout.module';
import { SharedModule } from '../../shared/shared.module';

import { ClientRoutingModule } from './client-routing.module';
import { DashboardComponent } from './dashboard/dashboard.component';

@NgModule({
  declarations: [DashboardComponent],
  imports: [CommonModule, SharedModule, LayoutModule, ClientRoutingModule]
})
export class ClientModule {}
