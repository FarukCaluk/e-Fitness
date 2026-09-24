import { Component, OnInit } from '@angular/core';
import { WorkoutPlansService } from '../../../core/services/workout-plans.service';
import { WorkoutPlanDetail, WorkoutPlanExerciseDetail } from '../../../core/models/workout-plan.model';
import { DayOfWeekPlan } from '../../../core/models/enums.model';

@Component({
  selector: 'app-my-workout',
  templateUrl: './my-workout.component.html',
  styleUrl: './my-workout.component.scss'
})
export class MyWorkoutComponent implements OnInit {
  plan: WorkoutPlanDetail | null = null;
  isLoading = true;

  readonly daysOfWeek = Object.values(DayOfWeekPlan);

  constructor(private readonly workoutPlansService: WorkoutPlansService) {}

  ngOnInit(): void {
    this.workoutPlansService.getPlans({ pageNumber: 1, pageSize: 1, isActive: true }).subscribe((result) => {
      const summary = result.items[0];

      if (!summary) {
        this.isLoading = false;
        return;
      }

      this.workoutPlansService.getPlan(summary.id).subscribe((plan) => {
        this.plan = plan;
        this.isLoading = false;
      });
    });
  }

  exercisesForDay(day: DayOfWeekPlan): WorkoutPlanExerciseDetail[] {
    return this.plan?.exercises.filter((e) => e.dayOfWeek === day) ?? [];
  }
}
