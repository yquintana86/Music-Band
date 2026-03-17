import { PagingFilter } from "../../shared/interfaces/paging-filter.interface";

export interface SearchMusicianByFilterQuery extends PagingFilter {
  firstName?: string;
  lastName?: string;
  fromAge?: number;
  toAge?: number;
  fromExperience?: number;
  toExperience?: number;
  fromBasicSalary?: number;
  toBasicSalary?: number;
  instrumentId?: number;
}
