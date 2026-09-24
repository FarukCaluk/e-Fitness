import { Component, Inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { TrainingSessionsService } from '../../../../core/services/training-sessions.service';

export interface ScheduleSessionDialogData {
  memberId: number;
}

@Component({
  selector: 'app-schedule-session-dialog',
  templateUrl: './schedule-session-dialog.component.html',
  styleUrl: './schedule-session-dialog.component.scss'
})
export class ScheduleSessionDialogComponent {
  readonly form;

  isSaving = false;

  constructor(
    private readonly fb: FormBuilder,
    private readonly trainingSessionsService: TrainingSessionsService,
    private readonly dialogRef: MatDialogRef<ScheduleSessionDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ScheduleSessionDialogData
  ) {
    this.form = this.fb.group({
      scheduledDate: [null as Date | null, Validators.required],
      scheduledTime: ['09:00', Validators.required],
      durationMinutes: [60, [Validators.required, Validators.min(15), Validators.max(240)]],
      location: [''],
      notes: ['']
    });
  }

  save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const raw = this.form.getRawValue();
    const [hours, minutes] = raw.scheduledTime!.split(':').map(Number);
    const scheduledAt = new Date(raw.scheduledDate!);
    scheduledAt.setHours(hours, minutes, 0, 0);

    this.isSaving = true;

    this.trainingSessionsService
      .createSession({
        trainerId: 0,
        memberId: this.data.memberId,
        scheduledAt: scheduledAt.toISOString(),
        durationMinutes: raw.durationMinutes!,
        notes: raw.notes || null,
        location: raw.location || null
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
