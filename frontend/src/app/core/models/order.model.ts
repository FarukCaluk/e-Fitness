import { OrderStatus, PaymentMethod } from './enums.model';

export interface OrderItem {
  productId: number;
  productName: string;
  quantity: number;
  unitPrice: number;
}

export interface Order {
  id: number;
  memberId: number;
  memberName: string;
  orderDate: string;
  totalAmount: number;
  status: OrderStatus;
  shippingAddress?: string | null;
  items: OrderItem[];
}

export interface OrderItemRequestItem {
  productId: number;
  quantity: number;
}

export interface CreateOrderRequest {
  items: OrderItemRequestItem[];
  shippingAddress?: string | null;
  paymentMethod: PaymentMethod;
}
