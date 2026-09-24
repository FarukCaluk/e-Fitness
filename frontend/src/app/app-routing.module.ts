import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'landing' },
  {
    path: 'landing',
    loadChildren: () => import('./features/landing/landing.module').then((m) => m.LandingModule)
  },
  {
    path: 'auth',
    loadChildren: () => import('./features/auth/auth.module').then((m) => m.AuthModule)
  },
  {
    path: 'admin',
    loadChildren: () => import('./features/admin/admin.module').then((m) => m.AdminModule)
  },
  {
    path: 'trainer',
    loadChildren: () => import('./features/trainer/trainer.module').then((m) => m.TrainerModule)
  },
  {
    path: 'client',
    loadChildren: () => import('./features/client/client.module').then((m) => m.ClientModule)
  },
  { path: '**', redirectTo: 'landing' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
