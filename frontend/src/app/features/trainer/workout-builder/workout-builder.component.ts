import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MembersService } from '../../../core/services/members.service';
import { ExercisesService } from '../../../core/services/exercises.service';
import { WorkoutPlansService } from '../../../core/services/workout-plans.service';
import { MemberListItem } from '../../../core/models/member.model';
import { Exercise } from '../../../core/models/exercise.model';
import { WorkoutPlanListItem } from '../../../core/models/workout-plan.model';
import { DayOfWeekPlan } from '../../../core/models/enums.model';

@Component({
  selector: 'app-workout-builder',
  templateUrl: './workout-builder.component.html',
  styleUrl: './workout-builder.component.scss'
})
export class WorkoutBuilderComponent implements OnInit {
  clients: MemberListItem[] = [];
  exercises: Exercise[] = [];
  plans: WorkoutPlanListItem[] = [];
  isLoading = true;
  isSaving = false;

  readonly daysOfWeek = Object.values(DayOfWeekPlan);
  readonly form;

  constructor(
    private readonly fb: FormBuilder,
    private readonly route: ActivatedRoute,
    private readonly membersService: MembersService,
    private readonly exercisesService: ExercisesService,
    private readonly workoutPlansService: WorkoutPlansService,
    private readonly snackBar: MatSnackBar
  ) {
    this.form = this.fb.group({
      memberId: [null as number | null, Validators.required],
      title: ['', Validators.required],
      description: [''],
      startDate: [new Date(), Validators.required],
      endDate: [null as Date | null],
      exercises: this.fb.array([] as ReturnType<typeof this.buildExerciseGroup>[])
    });
  }

  ngOnInit(): void {
    const memberIdParam = this.route.snapshot.queryParamMap.get('memberId');

    this.membersService.getMyClients({ pageNumber: 1, pageSize: 100 }).subscribe((result) => {
      this.clients = result.items;
      if (memberIdParam) {
        this.form.patchValue({ memberId: Number(memberIdParam) });
      }
    });

    this.exercisesService.getExercises({ pageNumber: 1, pageSize: 100 }).subscribe((result) => {
      this.exercises = result.items;
    });

    this.loadPlans();
    this.addExerciseRow();
  }

  loadPlans(): void {
    this.isLoading = true;
    this.workoutPlansService.getPlans({ pageNumber: 1, pageSize: 20 }).subscribe((result) => {
      this.plans = result.items;
      this.isLoading = false;
    });
  }

  get exerciseRows(): FormArray {
    return this.form.get('exercises') as FormArray;
  }

  private buildExerciseGroup() {
    return this.fb.group({
      exerciseId: [null as number | null, Validators.required],
      dayOfWeek: [DayOfWeekPlan.Monday, Validators.required],
      setsCount: [3, [Validators.required, Validators.min(1)]],
      repsCount: [10, [Validators.required, Validators.min(1)]],
      restSeconds: [60, [Validators.required, Validators.min(0)]]
    });
  }

  addExerciseRow(): void {
    this.exerciseRows.push(this.buildExerciseGroup());
  }

  removeExerciseRow(index: number): void {
    this.exerciseRows.removeAt(index);
  }

  submit(): void {
    if (this.form.invalid || this.exerciseRows.length === 0) {
      this.form.markAllAsTouched();
      return;
    }

    this.isSaving = true;
    const raw = this.form.getRawValue();

    this.workoutPlansService
      .createPlan({
        trainerId: 0,
        memberId: raw.memberId!,
        title: raw.title!,
        description: raw.description || null,
        startDate: raw.startDate!.toISOString(),
        endDate: raw.endDate ? raw.endDate.toISOString() : null,
        exercises: raw.exercises!.map((exercise, index) => ({
          exerciseId: exercise.exerciseId!,
          dayOfWeek: exercise.dayOfWeek!,
          setsCount: exercise.setsCount!,
          repsCount: exercise.repsCount!,
          restSeconds: exercise.restSeconds!,
          orderIndex: index
        }))
      })
      .subscribe({
        next: () => {
          this.isSaving = false;
          this.snackBar.open('Workout plan created.', 'Close', { duration: 3000 });
          this.form.reset({ startDate: new Date() });
          this.exerciseRows.clear();
          this.addExerciseRow();
          this.loadPlans();
        },
        error: () => (this.isSaving = false)
      });
  }

  deletePlan(plan: WorkoutPlanListItem): void {
    this.workoutPlansService.deletePlan(plan.id).subscribe(() => {
      this.snackBar.open('Workout plan deleted.', 'Close', { duration: 3000 });
      this.loadPlans();
    });
  }
}
