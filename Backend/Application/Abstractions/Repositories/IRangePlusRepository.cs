
using Application.RangePlusses.Query.SearchRangePlusByFilter;
using Domain.Entities;
using SharedLib.Models.Common;

namespace Application.Abstractions.Repositories;

public interface IRangePlusRepository
{
    Task<IEnumerable<RangePlus>> GetAllAsync(CancellationToken cancellationToken);
    Task<PagedResult<RangePlus>> SearchByFilterAsync(SearchRangePlusByFilterQuery filter, CancellationToken cancellationToken);
    Task<bool> ExistAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> IsExperienceRangeWithConflictAsync(int minExperience, int maxExperience, CancellationToken cancellationToken = default);
    Task<RangePlus?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    void Add(RangePlus rangePlus, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);


}
