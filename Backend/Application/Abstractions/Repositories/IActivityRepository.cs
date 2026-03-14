using Application.Activities.Queries.SearchActivitiesByFilter;
using Domain.Entities;
using SharedLib.Models.Common;

namespace Application.Abstractions.Repositories;

public interface IActivityRepository
{
    Task<IEnumerable<Activity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PagedResult<Activity>> SearchByFilterAsync(SearchActivitiesByFilterQuery filter, CancellationToken cancellationToken = default);

    Task<Activity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistIdAsync(int id, CancellationToken cancellationToken = default);
    void Add(Activity activity);
    Task DeleteAsync(int id);
    Task<IEnumerable<Activity>> GetInternationalActivitiesByMusicianIdAsync(int id, CancellationToken cancellationToken);
}
