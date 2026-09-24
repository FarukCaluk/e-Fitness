import { EquipmentCategory } from './enums.model';

export interface Equipment {
  id: number;
  name: string;
  description?: string | null;
  category: EquipmentCategory;
  quantity: number;
  isAvailable: boolean;
}

export interface CreateEquipmentRequest {
  name: string;
  description?: string | null;
  category: EquipmentCategory;
  quantity: number;
}

export interface UpdateEquipmentRequest extends CreateEquipmentRequest {
  isAvailable: boolean;
}
