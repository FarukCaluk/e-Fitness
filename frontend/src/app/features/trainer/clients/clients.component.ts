import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { Router } from '@angular/router';
import { MembersService } from '../../../core/services/members.service';
import { MemberListItem } from '../../../core/models/member.model';
import { ScheduleSessionDialogComponent } from './schedule-session-dialog/schedule-session-dialog.component';

@Component({
  selector: 'app-clients',
  templateUrl: './clients.component.html',
  styleUrl: './clients.component.scss'
})
export class ClientsComponent implements OnInit {
  clients: MemberListItem[] = [];
  totalCount = 0;
  pageNumber = 1;
  pageSize = 10;
  searchTerm = '';
  isLoading = false;

  readonly displayedColumns = ['name', 'contact', 'plan', 'joined', 'actions'];

  constructor(
    private readonly membersService: MembersService,
    private readonly dialog: MatDialog,
    private readonly router: Router
  ) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.isLoading = true;
    this.membersService
      .getMyClients({ pageNumber: this.pageNumber, pageSize: this.pageSize, searchTerm: this.searchTerm || undefined })
      .subscribe((result) => {
        this.clients = result.items;
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

  scheduleSession(client: MemberListItem): void {
    this.dialog.open(ScheduleSessionDialogComponent, { width: '420px', data: { memberId: client.id } });
  }

  buildWorkout(client: MemberListItem): void {
    this.router.navigate(['/trainer/workout-builder'], { queryParams: { memberId: client.id } });
  }

  message(): void {
    this.router.navigate(['/trainer/chat']);
  }
}
