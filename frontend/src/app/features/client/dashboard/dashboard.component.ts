import { Component, OnInit } from '@angular/core';
import { forkJoin, of, switchMap } from 'rxjs';
import { MembersService } from '../../../core/services/members.service';
import { MembershipsService } from '../../../core/services/memberships.service';
import { TrainingSessionsService } from '../../../core/services/training-sessions.service';
import { TrainersService } from '../../../core/services/trainers.service';
import { Membership } from '../../../core/models/membership.model';
import { TrainingSession } from '../../../core/models/training-session.model';
import { TrainingSessionStatus, MembershipStatus } from '../../../core/models/enums.model';

@Component({
  selector: 'app-client-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {
  isLoading = true;
  activeMembership: Membership | null = null;
  nextSession: TrainingSession | null = null;
  trainerName: string | null = null;

  constructor(
    private readonly membersService: MembersService,
    private readonly membershipsService: MembershipsService,
    private readonly trainingSessionsService: TrainingSessionsService,
    private readonly trainersService: TrainersService
  ) {}

  ngOnInit(): void {
    forkJoin({
      membership: this.membershipsService.getMemberships({ pageNumber: 1, pageSize: 1, status: MembershipStatus.Active }),
      session: this.trainingSessionsService.getSessions({
        pageNumber: 1,
        pageSize: 1,
        status: TrainingSessionStatus.Scheduled,
        fromDate: new Date().toISOString()
      }),
      profile: this.membersService.getMyProfile().pipe(
        switchMap((profile) => (profile.assignedTrainerId ? this.trainersService.getTrainer(profile.assignedTrainerId) : of(null)))
      )
    }).subscribe((result) => {
      this.activeMembership = result.membership.items[0] ?? null;
      this.nextSession = result.session.items[0] ?? null;
      this.trainerName = result.profile ? `${result.profile.firstName} ${result.profile.lastName}` : null;
      this.isLoading = false;
    });
  }
}
