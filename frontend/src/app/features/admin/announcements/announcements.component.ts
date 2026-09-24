import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AnnouncementsService } from '../../../core/services/announcements.service';
import { Announcement } from '../../../core/models/announcement.model';
import { AnnouncementSegment } from '../../../core/models/enums.model';

@Component({
  selector: 'app-announcements',
  templateUrl: './announcements.component.html',
  styleUrl: './announcements.component.scss'
})
export class AnnouncementsComponent implements OnInit {
  announcements: Announcement[] = [];
  isLoading = false;
  isSaving = false;
  showForm = false;

  readonly segments = Object.values(AnnouncementSegment);
  readonly form;

  constructor(
    private readonly fb: FormBuilder,
    private readonly announcementsService: AnnouncementsService,
    private readonly snackBar: MatSnackBar
  ) {
    this.form = this.fb.group({
      title: ['', Validators.required],
      body: ['', Validators.required],
      segment: [AnnouncementSegment.AllMembers, Validators.required]
    });
  }

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.isLoading = true;
    this.announcementsService.getAnnouncements({ pageNumber: 1, pageSize: 20 }).subscribe((result) => {
      this.announcements = result.items;
      this.isLoading = false;
    });
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isSaving = true;
    const raw = this.form.getRawValue();

    this.announcementsService
      .createAnnouncement({ title: raw.title!, body: raw.body!, segment: raw.segment! })
      .subscribe({
        next: () => {
          this.isSaving = false;
          this.showForm = false;
          this.form.reset({ segment: AnnouncementSegment.AllMembers });
          this.snackBar.open('Announcement published.', 'Close', { duration: 3000 });
          this.load();
        },
        error: () => (this.isSaving = false)
      });
  }
}
