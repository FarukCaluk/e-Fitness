import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MembersService } from '../../../../core/services/members.service';
import { TrainersService } from '../../../../core/services/trainers.service';
import { TrainerListItem } from '../../../../core/models/trainer.model';

export interface MemberEditDialogData {
  memberId: number;
}

@Component({
  selector: 'app-member-edit-dialog',
  templateUrl: './member-edit-dialog.component.html',
  styleUrl: './member-edit-dialog.component.scss'
})
export class MemberEditDialogComponent implements OnInit {
  readonly form;

  trainers: TrainerListItem[] = [];
  isLoading = true;
  isSaving = false;

  constructor(
    private readonly fb: FormBuilder,
    private readonly membersService: MembersService,
    private readonly trainersService: TrainersService,
    private readonly dialogRef: MatDialogRef<MemberEditDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: MemberEditDialogData
  ) {
    this.form = this.fb.group({
      phoneNumber: [''],
      dateOfBirth: [null as Date | null],
      gender: [''],
      address: [''],
      city: [''],
      emergencyContactName: [''],
      emergencyContactPhone: [''],
      assignedTrainerId: [null as number | null]
    });
  }

  ngOnInit(): void {
    this.trainersService.getTrainers({ pageNumber: 1, pageSize: 100 }).subscribe((result) => {
      this.trainers = result.items;
    });

    this.membersService.getMember(this.data.memberId).subscribe((member) => {
      this.form.patchValue({
        phoneNumber: member.phoneNumber ?? '',
        dateOfBirth: member.dateOfBirth ? new Date(member.dateOfBirth) : null,
        gender: member.gender ?? '',
        address: member.address ?? '',
        city: member.city ?? '',
        emergencyContactName: member.emergencyContactName ?? '',
        emergencyContactPhone: member.emergencyContactPhone ?? '',
        assignedTrainerId: member.assignedTrainerId ?? null
      });
      this.isLoading = false;
    });
  }

  save(): void {
    this.isSaving = true;
    const raw = this.form.getRawValue();

    this.membersService
      .updateMember(this.data.memberId, {
        phoneNumber: raw.phoneNumber || null,
        dateOfBirth: raw.dateOfBirth ? raw.dateOfBirth.toISOString() : null,
        gender: raw.gender || null,
        address: raw.address || null,
        city: raw.city || null,
        emergencyContactName: raw.emergencyContactName || null,
        emergencyContactPhone: raw.emergencyContactPhone || null,
        assignedTrainerId: raw.assignedTrainerId || null
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
