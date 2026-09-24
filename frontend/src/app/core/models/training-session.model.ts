import { TrainingSessionStatus } from './enums.model';

export interface TrainingSession {
  id: number;
  trainerId: number;
  trainerName: string;
  memberId: number;
  memberName: string;
  scheduledAt: string;
  durationMinutes: number;
  status: TrainingSessionStatus;
  notes?: string | null;
  location?: string | null;
}

export interface CreateTrainingSessionRequest {
  trainerId: number;
  memberId: number;
  scheduledAt: string;
  durationMinutes: number;
  notes?: string | null;
  location?: string | null;
}
