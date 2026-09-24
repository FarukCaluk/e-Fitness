import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { authGuard } from '../../core/guards/auth.guard';
import { roleGuard } from '../../core/guards/role.guard';
import { UserRole } from '../../core/models/user-role.enum';
import { ShellComponent } from '../../layout/shell/shell.component';
import { DashboardComponent } from './dashboard/dashboard.component';

const routes: Routes = [
  {
    path: '',
    component: ShellComponent,
    canActivate: [authGuard, roleGuard],
    data: { roles: [UserRole.Trainer] },
    children: [{ path: '', component: DashboardComponent }]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TrainerRoutingModule {}
