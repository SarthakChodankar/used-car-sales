export interface CarImage {
  imageId: number;
  url: string;
  isPrimary: boolean;
}

export interface Car {
  carId: number;
  vin: string;
  make: string;
  model: string;
  manufactureYear: number;
  mileage: number;
  color: string | null;
  fuelType: string;
  transmission: string;
  price: number;
  status: 'Available' | 'Reserved' | 'Sold';
  description: string | null;
  primaryImageUrl: string | null;
  images: CarImage[];
}

export interface CarQuery {
  search?: string;
  status?: string;
  pageNumber: number;
  pageSize: number;
}

export interface CarSaveRequest {
  vin: string;
  make: string;
  model: string;
  manufactureYear: number;
  mileage: number;
  color: string | null;
  fuelType: string;
  transmission: string;
  price: number;
  description: string | null;
}