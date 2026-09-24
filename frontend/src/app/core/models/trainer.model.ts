export interface TrainerListItem {
  id: number;
  userId: number;
  fullName: string;
  email: string;
  specialization: string;
  yearsOfExperience: number;
  hourlyRate: number;
  rating: number;
  isAvailable: boolean;
}

export interface TrainerDetail {
  id: number;
  userId: number;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber?: string | null;
  specialization: string;
  bio?: string | null;
  yearsOfExperience: number;
  hourlyRate: number;
  rating: number;
  isAvailable: boolean;
}

export interface CreateTrainerRequest {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  phoneNumber?: string | null;
  specialization: string;
  bio?: string | null;
  yearsOfExperience: number;
  hourlyRate: number;
}

export interface UpdateTrainerRequest {
  specialization: string;
  bio?: string | null;
  yearsOfExperience: number;
  hourlyRate: number;
  isAvailable: boolean;
}
