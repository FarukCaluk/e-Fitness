import { DayOfWeekPlan } from './enums.model';

export interface WorkoutExerciseItem {
  exerciseId: number;
  dayOfWeek: DayOfWeekPlan;
  setsCount: number;
  repsCount: number;
  restSeconds: number;
  orderIndex: number;
}

export interface WorkoutPlanListItem {
  id: number;
  title: string;
  trainerId: number;
  trainerName: string;
  memberId: number;
  memberName: string;
  startDate: string;
  endDate?: string | null;
  isActive: boolean;
}

export interface WorkoutPlanExerciseDetail extends WorkoutExerciseItem {
  id: number;
  exerciseName: string;
}

export interface WorkoutPlanDetail {
  id: number;
  title: string;
  description?: string | null;
  trainerId: number;
  trainerName: string;
  memberId: number;
  memberName: string;
  startDate: string;
  endDate?: string | null;
  isActive: boolean;
  exercises: WorkoutPlanExerciseDetail[];
}

export interface CreateWorkoutPlanRequest {
  trainerId: number;
  memberId: number;
  title: string;
  description?: string | null;
  startDate: string;
  endDate?: string | null;
  exercises: WorkoutExerciseItem[];
}

export interface UpdateWorkoutPlanRequest {
  title: string;
  description?: string | null;
  startDate: string;
  endDate?: string | null;
  isActive: boolean;
  exercises: WorkoutExerciseItem[];
}
