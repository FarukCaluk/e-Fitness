import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ProgressLogsService } from '../../../core/services/progress-logs.service';
import { ProgressLog } from '../../../core/models/progress-log.model';

@Component({
  selector: 'app-progress',
  templateUrl: './progress.component.html',
  styleUrl: './progress.component.scss'
})
export class ProgressComponent implements OnInit {
  logs: ProgressLog[] = [];
  isLoading = true;
  isSaving = false;
  showForm = false;

  readonly form;

  constructor(
    private readonly fb: FormBuilder,
    private readonly progressLogsService: ProgressLogsService,
    private readonly snackBar: MatSnackBar
  ) {
    this.form = this.fb.group({
      weightKg: [70, [Validators.required, Validators.min(1)]],
      bodyFatPercentage: [null as number | null],
      muscleMassKg: [null as number | null],
      notes: ['']
    });
  }

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.isLoading = true;
    this.progressLogsService.getProgressLogs({ pageNumber: 1, pageSize: 100 }).subscribe((result) => {
      this.logs = result.items;
      this.isLoading = false;
    });
  }

  get chartPoints(): string {
    if (this.logs.length < 2) {
      return '';
    }

    const weights = this.logs.map((l) => l.weightKg);
    const min = Math.min(...weights);
    const max = Math.max(...weights);
    const range = max - min || 1;

    return this.logs
      .map((log, index) => {
        const x = (index / (this.logs.length - 1)) * 100;
        const y = 100 - ((log.weightKg - min) / range) * 100;
        return `${x},${y}`;
      })
      .join(' ');
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isSaving = true;
    const raw = this.form.getRawValue();

    this.progressLogsService
      .createProgressLog({
        weightKg: raw.weightKg!,
        bodyFatPercentage: raw.bodyFatPercentage,
        muscleMassKg: raw.muscleMassKg,
        notes: raw.notes || null
      })
      .subscribe({
        next: () => {
          this.isSaving = false;
          this.showForm = false;
          this.form.reset({ weightKg: raw.weightKg });
          this.snackBar.open('Progress logged.', 'Close', { duration: 3000 });
          this.load();
        },
        error: () => (this.isSaving = false)
      });
  }
}
