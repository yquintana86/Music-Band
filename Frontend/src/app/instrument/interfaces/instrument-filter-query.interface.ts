import { InstrumentType } from "./instrument-type.enum";

export interface InstrumentFilterQuery {
  name?: string;
  country?: string;
  instrumentType?: InstrumentType;
  page: number;
  pageSize: number;
  requestCount?: boolean;
}
