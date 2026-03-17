import { PagingFilter } from "../../shared/interfaces/paging-filter.interface";

export interface ActivityFilterQuery extends PagingFilter{
    international?: boolean;
    begin?: Date;
    end?: Date;
}
