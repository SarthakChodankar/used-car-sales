export interface SaleRequest {
  carId: number;
  customerId: number;
  salePrice: number;
  paymentMethod: string;
}

export interface Bill {
  billId: number;
  invoiceNumber: string;
  billDate: string;
  salePrice: number;
  taxPercent: number;
  taxAmount: number;
  totalAmount: number;
  paymentMethod: string;
  car: { carId: number; vin: string; make: string; model: string; manufactureYear: number };
  customer: { customerId: number; fullName: string; email: string; phone: string; address: string | null };
}

export interface BillQuery {
  search?: string;
  pageNumber: number;
  pageSize: number;
}