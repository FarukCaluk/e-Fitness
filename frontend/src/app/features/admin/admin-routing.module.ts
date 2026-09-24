import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { authGuard } from '../../core/guards/auth.guard';
import { roleGuard } from '../../core/guards/role.guard';
import { UserRole } from '../../core/models/user-role.enum';
import { ShellComponent } from '../../layout/shell/shell.component';
import { AnnouncementsComponent } from './announcements/announcements.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { FinanceComponent } from './finance/finance.component';
import { MembersComponent } from './members/members.component';
import { PlansComponent } from './plans/plans.component';
import { ShopComponent } from './shop/shop.component';
import { TrainersComponent } from './trainers/trainers.component';

const routes: Routes = [
  {
    path: '',
    component: ShellComponent,
    canActivate: [authGuard, roleGuard],
    data: { roles: [UserRole.Admin] },
    children: [
      { path: '', component: DashboardComponent },
      { path: 'members', component: MembersComponent },
      { path: 'trainers', component: TrainersComponent },
      { path: 'plans', component: PlansComponent },
      { path: 'shop', component: ShopComponent },
      { path: 'finance', component: FinanceComponent },
      { path: 'announcements', component: AnnouncementsComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule {}
