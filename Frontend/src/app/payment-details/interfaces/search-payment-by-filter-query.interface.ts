import { PagingFilter } from "../../shared/interfaces/paging-filter.interface";

export interface SearchPaymentDetailsByFilterQuery extends PagingFilter{
  fromPaymentDate?: Date;
  toPaymentDate?: Date;
  fromSalary?: number;
  toSalary?: number;
}
