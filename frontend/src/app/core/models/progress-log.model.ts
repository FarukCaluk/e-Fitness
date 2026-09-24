export interface ProgressLog {
  id: number;
  recordedAt: string;
  weightKg: number;
  bodyFatPercentage?: number | null;
  muscleMassKg?: number | null;
  notes?: string | null;
}

export interface CreateProgressLogRequest {
  weightKg: number;
  bodyFatPercentage?: number | null;
  muscleMassKg?: number | null;
  notes?: string | null;
}
