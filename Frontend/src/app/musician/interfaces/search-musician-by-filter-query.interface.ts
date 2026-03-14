export interface SearchMusicianByFilterQuery{
  firstName?: string;
  lastName?: string;
  fromAge?: number;
  toAge?: number;
  fromExperience?: number;
  toExperience?: number;
  fromBasicSalary?: number;
  toBasicSalary?: number;
  instrumentId?: number;
  page: number;
  pageSize: number;
  requestCount?: boolean;
}
