using Microsoft.EntityFrameworkCore;
using SharedLib.Models.Common;
using System.Linq.Expressions;

namespace Infrastructure.Extensions;

internal static class SQLPagedListExtentions
{
    internal static async Task<PagedResult<T>> ToQuickPageList<T, Key>(this IQueryable<T> query, Expression<Func<T, Key>> orderBy,
        int currentPage, int pageSize, bool? requestPaging, CancellationToken cancellationToken)
    {
     
        bool hasNextPage = false;
        int count = 0;
        int? totalItemCount = null;

        PagedResult<T> paged = new PagedResult<T>();

        if (requestPaging.HasValue && requestPaging.Value)
        {
            totalItemCount = await query.CountAsync(cancellationToken);
        }

        var result = await query.OrderBy(orderBy).Skip((currentPage - 1) * pageSize).Take(pageSize + 1).ToListAsync(cancellationToken);

        count = result.Count;

        if (count != 0)
        {
            if (count == pageSize + 1)
            {
                hasNextPage = true;
                result.RemoveAt(pageSize);
            }

            paged = new PagedResult<T>
            {
                Currentpage = currentPage,
                PageSize = pageSize,
                HasNextPage = hasNextPage,
                ItemCount = !hasNextPage ? count : count - 1,
                TotalItemCount = totalItemCount,
                PageCount = totalItemCount.HasValue ? (int)Math.Ceiling((double)totalItemCount / pageSize) : totalItemCount,
                Results = result?.ToList()
            };
        }

        return paged;

    }

}
