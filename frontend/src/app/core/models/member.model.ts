import { MembershipStatus } from './enums.model';

export interface MemberListItem {
  id: number;
  fullName: string;
  email: string;
  phoneNumber?: string | null;
  city?: string | null;
  joinDate: string;
  currentMembershipStatus?: MembershipStatus | null;
  currentPlanName?: string | null;
  assignedTrainerName?: string | null;
}

export interface MemberDetail {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber?: string | null;
  dateOfBirth?: string | null;
  gender?: string | null;
  address?: string | null;
  city?: string | null;
  emergencyContactName?: string | null;
  emergencyContactPhone?: string | null;
  joinDate: string;
  assignedTrainerId?: number | null;
}

export interface UpdateMemberRequest {
  phoneNumber?: string | null;
  dateOfBirth?: string | null;
  gender?: string | null;
  address?: string | null;
  city?: string | null;
  emergencyContactName?: string | null;
  emergencyContactPhone?: string | null;
  assignedTrainerId?: number | null;
}
