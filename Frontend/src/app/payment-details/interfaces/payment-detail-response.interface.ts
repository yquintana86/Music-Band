export interface PaymentDetailResponse {
  id: number;
  paymentDate?: Date;
  salary: number;
  basicSalary: number;
  musicianId: number;
  musicianName: string;
  rangePlusId: number;
  description?: string;
}
