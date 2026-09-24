import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { TrainersService } from '../../../../core/services/trainers.service';

export interface TrainerFormDialogData {
  trainerId: number;
}

@Component({
  selector: 'app-trainer-form-dialog',
  templateUrl: './trainer-form-dialog.component.html',
  styleUrl: './trainer-form-dialog.component.scss'
})
export class TrainerFormDialogComponent implements OnInit {
  readonly isEditMode: boolean;
  readonly form;

  isLoading: boolean;
  isSaving = false;

  constructor(
    private readonly fb: FormBuilder,
    private readonly trainersService: TrainersService,
    private readonly dialogRef: MatDialogRef<TrainerFormDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: TrainerFormDialogData | null
  ) {
    this.isEditMode = !!this.data;
    this.isLoading = this.isEditMode;

    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', this.isEditMode ? [] : [Validators.required, Validators.minLength(8)]],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      phoneNumber: [''],
      specialization: ['', Validators.required],
      bio: [''],
      yearsOfExperience: [0, [Validators.required, Validators.min(0)]],
      hourlyRate: [0, [Validators.required, Validators.min(0)]],
      isAvailable: [true]
    });
  }

  ngOnInit(): void {
    if (!this.isEditMode || !this.data) {
      return;
    }

    this.trainersService.getTrainer(this.data.trainerId).subscribe((trainer) => {
      this.form.patchValue({
        email: trainer.email,
        firstName: trainer.firstName,
        lastName: trainer.lastName,
        phoneNumber: trainer.phoneNumber ?? '',
        specialization: trainer.specialization,
        bio: trainer.bio ?? '',
        yearsOfExperience: trainer.yearsOfExperience,
        hourlyRate: trainer.hourlyRate,
        isAvailable: trainer.isAvailable
      });
      this.form.get('email')?.disable();
      this.isLoading = false;
    });
  }

  save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isSaving = true;
    const raw = this.form.getRawValue();

    if (this.isEditMode && this.data) {
      this.trainersService
        .updateTrainer(this.data.trainerId, {
          specialization: raw.specialization!,
          bio: raw.bio || null,
          yearsOfExperience: raw.yearsOfExperience!,
          hourlyRate: raw.hourlyRate!,
          isAvailable: raw.isAvailable!
        })
        .subscribe({
          next: () => this.dialogRef.close(true),
          error: () => (this.isSaving = false)
        });
      return;
    }

    this.trainersService
      .createTrainer({
        email: raw.email!,
        password: raw.password!,
        firstName: raw.firstName!,
        lastName: raw.lastName!,
        phoneNumber: raw.phoneNumber || null,
        specialization: raw.specialization!,
        bio: raw.bio || null,
        yearsOfExperience: raw.yearsOfExperience!,
        hourlyRate: raw.hourlyRate!
      })
      .subscribe({
        next: () => this.dialogRef.close(true),
        error: () => (this.isSaving = false)
      });
  }

  close(): void {
    this.dialogRef.close(false);
  }
}
