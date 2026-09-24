import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { authGuard } from '../../core/guards/auth.guard';
import { roleGuard } from '../../core/guards/role.guard';
import { UserRole } from '../../core/models/user-role.enum';
import { ShellComponent } from '../../layout/shell/shell.component';
import { ChatComponent } from '../../shared/chat/chat.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { MyWorkoutComponent } from './my-workout/my-workout.component';
import { PlansComponent } from './plans/plans.component';
import { ProgressComponent } from './progress/progress.component';
import { ShopComponent } from './shop/shop.component';

const routes: Routes = [
  {
    path: '',
    component: ShellComponent,
    canActivate: [authGuard, roleGuard],
    data: { roles: [UserRole.Client] },
    children: [
      { path: '', component: DashboardComponent },
      { path: 'my-workout', component: MyWorkoutComponent },
      { path: 'progress', component: ProgressComponent },
      { path: 'chat', component: ChatComponent },
      { path: 'shop', component: ShopComponent },
      { path: 'plans', component: PlansComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ClientRoutingModule {}
