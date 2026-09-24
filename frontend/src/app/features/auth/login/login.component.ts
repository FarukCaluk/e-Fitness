import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { UserRole } from '../../../core/models/user-role.enum';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  readonly form;

  isSubmitting = false;
  errorMessage: string | null = null;

  constructor(
    private readonly fb: FormBuilder,
    private readonly authService: AuthService,
    private readonly router: Router
  ) {
    this.form = this.fb.nonNullable.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]]
    });
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = null;

    this.authService.login(this.form.getRawValue()).subscribe({
      next: (response) => {
        this.isSubmitting = false;
        this.router.navigate([this.getHomeRouteForRole(response.user.role)]);
      },
      error: () => {
        this.isSubmitting = false;
        this.errorMessage = 'Invalid email or password.';
      }
    });
  }

  private getHomeRouteForRole(role: UserRole): string {
    switch (role) {
      case UserRole.Admin:
        return '/admin';
      case UserRole.Trainer:
        return '/trainer';
      default:
        return '/client';
    }
  }
}
