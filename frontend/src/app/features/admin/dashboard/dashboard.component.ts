import { Component, OnInit } from '@angular/core';
import { forkJoin } from 'rxjs';
import { MembersService } from '../../../core/services/members.service';
import { TrainersService } from '../../../core/services/trainers.service';
import { MembershipsService } from '../../../core/services/memberships.service';
import { OrdersService } from '../../../core/services/orders.service';
import { AnnouncementsService } from '../../../core/services/announcements.service';
import { Announcement } from '../../../core/models/announcement.model';
import { MembershipStatus, OrderStatus } from '../../../core/models/enums.model';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {
  isLoading = true;
  totalMembers = 0;
  totalTrainers = 0;
  activeMemberships = 0;
  pendingOrders = 0;
  recentAnnouncements: Announcement[] = [];

  constructor(
    private readonly membersService: MembersService,
    private readonly trainersService: TrainersService,
    private readonly membershipsService: MembershipsService,
    private readonly ordersService: OrdersService,
    private readonly announcementsService: AnnouncementsService
  ) {}

  ngOnInit(): void {
    forkJoin({
      members: this.membersService.getMembers({ pageNumber: 1, pageSize: 1 }),
      trainers: this.trainersService.getTrainers({ pageNumber: 1, pageSize: 1 }),
      memberships: this.membershipsService.getMemberships({ pageNumber: 1, pageSize: 1, status: MembershipStatus.Active }),
      orders: this.ordersService.getOrders({ pageNumber: 1, pageSize: 1, status: OrderStatus.Pending }),
      announcements: this.announcementsService.getAnnouncements({ pageNumber: 1, pageSize: 5 })
    }).subscribe((result) => {
      this.totalMembers = result.members.totalCount;
      this.totalTrainers = result.trainers.totalCount;
      this.activeMemberships = result.memberships.totalCount;
      this.pendingOrders = result.orders.totalCount;
      this.recentAnnouncements = result.announcements.items;
      this.isLoading = false;
    });
  }
}
