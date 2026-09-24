import { MembershipStatus, PaymentMethod } from './enums.model';

export interface Membership {
  id: number;
  memberId: number;
  memberName: string;
  membershipPlanId: number;
  planName: string;
  startDate: string;
  endDate: string;
  status: MembershipStatus;
  autoRenew: boolean;
}

export interface SubscribeToMembershipRequest {
  membershipPlanId: number;
  paymentMethod: PaymentMethod;
  autoRenew: boolean;
}
