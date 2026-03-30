using Application.Activities.Queries.Dtos;
using Application.Activities.Queries.SearchActivitiesByFilter;
using Domain.Entities;
using SharedLib.Models.Common;
using System.Linq.Expressions;

namespace Application.Abstractions.Repositories;

public interface IActivityRepository
{
    Task<IEnumerable<Activity>> GetAllAsync(CancellationToken cancellationToken = default);   
    Task<PagedResult<Activity>> SearchByFilterAsync(SearchActivitiesByFilterQuery filter, CancellationToken cancellationToken = default,
        params Expression<Func<Activity, Object>>[] includes);
    
    //Task<PagedResult<ActivityMusiciansNameDto>> SearchAsync(SearchActivitiesByFilterQuery filter, CancellationToken cancellationToken = default,
    //    params Expression<Func<Activity, Object>>[] includes);


    Task<Activity?> GetByIdAsync(int id, CancellationToken cancellationToken = default, params Expression<Func<Activity, object>>[] includes);
    Task<bool> ExistIdAsync(int id, CancellationToken cancellationToken = default);
    void Add(Activity activity);
    Task DeleteAsync(int id);
    Task DeleteManyAsync(List<int> id);
    Task<IEnumerable<Activity>> GetInternationalActivitiesByMusicianIdAsync(int id, CancellationToken cancellationToken);
}
