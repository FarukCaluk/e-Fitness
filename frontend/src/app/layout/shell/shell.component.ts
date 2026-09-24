import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { AuthenticatedUser } from '../../core/models/auth.model';
import { UserRole } from '../../core/models/user-role.enum';

interface NavItem {
  path: string;
  icon: string;
  label: string;
}

const NAV_ITEMS_BY_ROLE: Record<UserRole, NavItem[]> = {
  [UserRole.Admin]: [
    { path: '', icon: 'space_dashboard', label: 'Dashboard' },
    { path: 'members', icon: 'group', label: 'Members' },
    { path: 'trainers', icon: 'fitness_center', label: 'Trainers' },
    { path: 'plans', icon: 'star', label: 'Membership Plans' },
    { path: 'shop', icon: 'storefront', label: 'Shop Inventory' },
    { path: 'finance', icon: 'bar_chart', label: 'Finance' },
    { path: 'announcements', icon: 'campaign', label: 'Announcements' }
  ],
  [UserRole.Trainer]: [
    { path: '', icon: 'space_dashboard', label: 'Dashboard' },
    { path: 'clients', icon: 'group', label: 'Clients' },
    { path: 'workout-builder', icon: 'fitness_center', label: 'Workout Builder' },
    { path: 'chat', icon: 'chat', label: 'Messages' }
  ],
  [UserRole.Client]: [
    { path: '', icon: 'space_dashboard', label: 'Dashboard' },
    { path: 'my-workout', icon: 'fitness_center', label: 'My Workout' },
    { path: 'progress', icon: 'bar_chart', label: 'Progress' },
    { path: 'chat', icon: 'chat', label: 'Chat' },
    { path: 'shop', icon: 'storefront', label: 'Shop' },
    { path: 'plans', icon: 'star', label: 'Membership' }
  ]
};

@Component({
  selector: 'app-shell',
  templateUrl: './shell.component.html',
  styleUrl: './shell.component.scss'
})
export class ShellComponent {
  currentUser: AuthenticatedUser | null = null;
  navItems: NavItem[] = [];

  constructor(private readonly authService: AuthService, private readonly router: Router) {
    this.currentUser = this.authService.getCurrentUser();
    this.navItems = this.currentUser ? NAV_ITEMS_BY_ROLE[this.currentUser.role] : [];
  }

  get initials(): string {
    if (!this.currentUser) {
      return '';
    }

    return `${this.currentUser.firstName.charAt(0)}${this.currentUser.lastName.charAt(0)}`.toUpperCase();
  }

  logout(): void {
    this.authService.logout().subscribe({
      complete: () => this.router.navigate(['/landing'])
    });
  }
}
