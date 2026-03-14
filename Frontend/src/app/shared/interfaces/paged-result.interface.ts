export interface PagedResult<T>{
  currentpage: number;
  pageCount?: number;
  pageSize: number;
  itemCount: number;
  totalItemCount?: number;
  hasNextPage: boolean;
  displayFrom: number;
  displayTo: number;
  displayTotal?: number;
  results: T[];
}
