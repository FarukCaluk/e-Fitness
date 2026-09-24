import { Component, OnInit } from '@angular/core';
import { forkJoin } from 'rxjs';
import { MembersService } from '../../../core/services/members.service';
import { TrainingSessionsService } from '../../../core/services/training-sessions.service';
import { TrainingSessionStatus } from '../../../core/models/enums.model';
import { TrainingSession } from '../../../core/models/training-session.model';

@Component({
  selector: 'app-trainer-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {
  isLoading = true;
  totalClients = 0;
  upcomingSessions: TrainingSession[] = [];

  constructor(
    private readonly membersService: MembersService,
    private readonly trainingSessionsService: TrainingSessionsService
  ) {}

  ngOnInit(): void {
    forkJoin({
      clients: this.membersService.getMyClients({ pageNumber: 1, pageSize: 1 }),
      sessions: this.trainingSessionsService.getSessions({
        pageNumber: 1,
        pageSize: 5,
        status: TrainingSessionStatus.Scheduled,
        fromDate: new Date().toISOString()
      })
    }).subscribe((result) => {
      this.totalClients = result.clients.totalCount;
      this.upcomingSessions = result.sessions.items;
      this.isLoading = false;
    });
  }
}
