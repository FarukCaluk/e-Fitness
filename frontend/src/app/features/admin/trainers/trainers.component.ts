import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TrainersService } from '../../../core/services/trainers.service';
import { TrainerListItem } from '../../../core/models/trainer.model';
import { TrainerFormDialogComponent } from './trainer-form-dialog/trainer-form-dialog.component';

@Component({
  selector: 'app-trainers',
  templateUrl: './trainers.component.html',
  styleUrl: './trainers.component.scss'
})
export class TrainersComponent implements OnInit {
  trainers: TrainerListItem[] = [];
  totalCount = 0;
  pageNumber = 1;
  pageSize = 10;
  searchTerm = '';
  isLoading = false;

  readonly displayedColumns = ['name', 'specialization', 'experience', 'rate', 'rating', 'status', 'actions'];

  constructor(
    private readonly trainersService: TrainersService,
    private readonly dialog: MatDialog,
    private readonly snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.isLoading = true;
    this.trainersService
      .getTrainers({ pageNumber: this.pageNumber, pageSize: this.pageSize, searchTerm: this.searchTerm || undefined })
      .subscribe((result) => {
        this.trainers = result.items;
        this.totalCount = result.totalCount;
        this.isLoading = false;
      });
  }

  onSearchChange(): void {
    this.pageNumber = 1;
    this.load();
  }

  onPageChange(event: PageEvent): void {
    this.pageNumber = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.load();
  }

  create(): void {
    const dialogRef = this.dialog.open(TrainerFormDialogComponent, { width: '480px', data: null });
    dialogRef.afterClosed().subscribe((saved) => {
      if (saved) {
        this.load();
      }
    });
  }

  edit(trainer: TrainerListItem): void {
    const dialogRef = this.dialog.open(TrainerFormDialogComponent, { width: '480px', data: { trainerId: trainer.id } });
    dialogRef.afterClosed().subscribe((saved) => {
      if (saved) {
        this.load();
      }
    });
  }

  remove(trainer: TrainerListItem): void {
    this.trainersService.deleteTrainer(trainer.id).subscribe(() => {
      this.snackBar.open(`${trainer.fullName} deactivated.`, 'Close', { duration: 3000 });
      this.load();
    });
  }
}
