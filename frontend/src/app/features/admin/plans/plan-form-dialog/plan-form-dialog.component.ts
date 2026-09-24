import { Component, Inject } from '@angular/core';
import { FormArray, FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MembershipPlansService } from '../../../../core/services/membership-plans.service';
import { MembershipPlan } from '../../../../core/models/membership-plan.model';

export interface PlanFormDialogData {
  plan: MembershipPlan;
}

@Component({
  selector: 'app-plan-form-dialog',
  templateUrl: './plan-form-dialog.component.html',
  styleUrl: './plan-form-dialog.component.scss'
})
export class PlanFormDialogComponent {
  readonly isEditMode: boolean;
  readonly form;

  isSaving = false;

  constructor(
    private readonly fb: FormBuilder,
    private readonly plansService: MembershipPlansService,
    private readonly dialogRef: MatDialogRef<PlanFormDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: PlanFormDialogData | null
  ) {
    this.isEditMode = !!this.data;

    this.form = this.fb.group({
      name: [this.data?.plan.name ?? '', Validators.required],
      description: [this.data?.plan.description ?? ''],
      price: [this.data?.plan.price ?? 0, [Validators.required, Validators.min(0)]],
      durationInDays: [this.data?.plan.durationInDays ?? 30, [Validators.required, Validators.min(1)]],
      isFeatured: [this.data?.plan.isFeatured ?? false],
      isActive: [this.data?.plan.isActive ?? true],
      features: this.fb.array((this.data?.plan.features ?? ['']).map((f) => this.fb.control(f, Validators.required)))
    });
  }

  get features(): FormArray {
    return this.form.get('features') as FormArray;
  }

  addFeature(): void {
    this.features.push(this.fb.control('', Validators.required));
  }

  removeFeature(index: number): void {
    this.features.removeAt(index);
  }

  save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isSaving = true;
    const raw = this.form.getRawValue();
    const payload = {
      name: raw.name!,
      description: raw.description || null,
      price: raw.price!,
      durationInDays: raw.durationInDays!,
      isFeatured: raw.isFeatured!,
      features: raw.features as string[]
    };

    if (this.isEditMode) {
      this.plansService.updatePlan(this.data!.plan.id, { ...payload, isActive: raw.isActive! }).subscribe({
        next: () => this.dialogRef.close(true),
        error: () => (this.isSaving = false)
      });
      return;
    }

    this.plansService.createPlan(payload).subscribe({
      next: () => this.dialogRef.close(true),
      error: () => (this.isSaving = false)
    });
  }

  close(): void {
    this.dialogRef.close(false);
  }
}
