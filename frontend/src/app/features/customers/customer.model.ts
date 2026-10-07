export interface Customer {
  customerId: number;
  fullName: string;
  email: string;
  phone: string;
  address: string | null;
  createdAt: string;
}

export interface CustomerSaveRequest {
  fullName: string;
  email: string;
  phone: string;
  address: string | null;
}

export interface CustomerQuery {
  search?: string;
  pageNumber: number;
  pageSize: number;
}