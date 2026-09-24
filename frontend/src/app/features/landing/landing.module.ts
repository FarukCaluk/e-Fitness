import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';

import { LandingPageComponent } from './landing-page/landing-page.component';
import { LandingRoutingModule } from './landing-routing.module';

@NgModule({
  declarations: [LandingPageComponent],
  imports: [CommonModule, SharedModule, LandingRoutingModule]
})
export class LandingModule {}
