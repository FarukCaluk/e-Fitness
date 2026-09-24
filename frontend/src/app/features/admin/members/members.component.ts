import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { MembersService } from '../../../core/services/members.service';
import { MemberListItem } from '../../../core/models/member.model';
import { MembershipStatus } from '../../../core/models/enums.model';
import { MemberEditDialogComponent } from './member-edit-dialog/member-edit-dialog.component';

@Component({
  selector: 'app-members',
  templateUrl: './members.component.html',
  styleUrl: './members.component.scss'
})
export class MembersComponent implements OnInit {
  members: MemberListItem[] = [];
  totalCount = 0;
  pageNumber = 1;
  pageSize = 10;
  searchTerm = '';
  statusFilter: MembershipStatus | '' = '';
  isLoading = false;

  readonly statuses = Object.values(MembershipStatus);
  readonly displayedColumns = ['name', 'contact', 'plan', 'trainer', 'joined', 'actions'];

  constructor(private readonly membersService: MembersService, private readonly dialog: MatDialog) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.isLoading = true;
    this.membersService
      .getMembers({
        pageNumber: this.pageNumber,
        pageSize: this.pageSize,
        searchTerm: this.searchTerm || undefined,
        membershipStatus: this.statusFilter || undefined
      })
      .subscribe((result) => {
        this.members = result.items;
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

  edit(member: MemberListItem): void {
    const dialogRef = this.dialog.open(MemberEditDialogComponent, { width: '480px', data: { memberId: member.id } });
    dialogRef.afterClosed().subscribe((updated) => {
      if (updated) {
        this.load();
      }
    });
  }
}
