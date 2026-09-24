export interface MembershipPlan {
  id: number;
  name: string;
  description?: string | null;
  price: number;
  durationInDays: number;
  isFeatured: boolean;
  isActive: boolean;
  features: string[];
}

export interface CreateMembershipPlanRequest {
  name: string;
  description?: string | null;
  price: number;
  durationInDays: number;
  isFeatured: boolean;
  features: string[];
}

export interface UpdateMembershipPlanRequest extends CreateMembershipPlanRequest {
  isActive: boolean;
}
