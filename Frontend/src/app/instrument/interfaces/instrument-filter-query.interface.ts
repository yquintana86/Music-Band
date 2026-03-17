import { PagingFilter } from "../../shared/interfaces/paging-filter.interface";
import { InstrumentType } from "./instrument-type.enum";

export interface InstrumentFilterQuery extends PagingFilter {
  name?: string;
  country?: string;
  instrumentType?: InstrumentType;
}
