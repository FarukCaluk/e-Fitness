import { ExerciseDifficulty, MuscleGroup } from './enums.model';

export interface Exercise {
  id: number;
  name: string;
  description?: string | null;
  muscleGroup: MuscleGroup;
  difficulty: ExerciseDifficulty;
  videoUrl?: string | null;
  equipmentId?: number | null;
  equipmentName?: string | null;
}

export interface CreateExerciseRequest {
  name: string;
  description?: string | null;
  muscleGroup: MuscleGroup;
  difficulty: ExerciseDifficulty;
  videoUrl?: string | null;
  equipmentId?: number | null;
}

export type UpdateExerciseRequest = CreateExerciseRequest;
