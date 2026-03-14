namespace SharedLib.Models.Common;

public class PagingFilter
{
    public PagingFilter()
    {
        ResetPagination();
    }

    public int Page { get; set; }
    public int PageSize { get; set; }
    public bool? RequestCount { get; set; }

    public void ResetPagination()
    {
        Page = 1;
        PageSize = 25;
        RequestCount = null;
    }

}
