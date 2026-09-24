import { PaymentMethod, PaymentPurpose, PaymentStatus } from './enums.model';

export interface Payment {
  id: number;
  memberId: number;
  memberName: string;
  amount: number;
  paymentDate: string;
  method: PaymentMethod;
  status: PaymentStatus;
  purpose: PaymentPurpose;
  transactionReference?: string | null;
}
