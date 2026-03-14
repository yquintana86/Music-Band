namespace SharedLib.Models.Common;

public abstract class PagedResult
{
    public int Currentpage { get; set; }
    public int? PageCount { get; set; }
    public int PageSize { get; set; }
    public int ItemCount { get; set; }
    public int? TotalItemCount { get; set; }
    public bool HasNextPage { get; set; }
    public int DisplayFrom => ItemCount > 0 ? (Currentpage - 1) * PageSize + 1 : 0;
    public int DisplayTo => ItemCount > 0 ? DisplayFrom + ItemCount - 1 : 0;
    public int? DisplayTotal => TotalItemCount.HasValue ? TotalItemCount.Value : HasNextPage ? int.MaxValue : DisplayTo;
}

public class PagedResult<T> : PagedResult
{
    private IReadOnlyList<T>? _results;
    public PagedResult()
    {
        PageSize = 25;
        Currentpage = 1;
        _results = new List<T>();
    }

    public IReadOnlyList<T>? Results
    {
        get { return _results; }
        init { _results = value is null ? 
                                   null : 
                                   new List<T>(value); }
    }   
}
