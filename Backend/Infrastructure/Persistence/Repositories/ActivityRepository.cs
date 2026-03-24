using Application.Abstractions.DataContext;
using Application.Abstractions.Repositories;
using Application.Activities.Queries.SearchActivitiesByFilter;
using Domain.Entities;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using SharedLib.Models.Common;

namespace Infrastructure.Persistence.Repositories;

internal class ActivityRepository : IActivityRepository
{
    private readonly IAppDbContext _appDbContext;

    public ActivityRepository(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public void Add(Activity activity)
    {
        _appDbContext.Activities.Add(activity);
    }

    public async Task DeleteAsync(int id)
    {
        var activity = await _appDbContext.Activities.FindAsync(id);
        ArgumentNullException.ThrowIfNull(activity);
        
        _appDbContext.Activities.Remove(activity);
    }

    public async Task DeleteManyAsync(List<int> ids)
    {
        var founded = await _appDbContext.Activities
            .Where(a => ids.Contains(a.Id))
            .ToListAsync();

        var foundedHashSet = founded.Select(a => a.Id).ToHashSet();

        var notFounded = ids.Where(id => !foundedHashSet.Contains(id)).ToList();
        if (notFounded.Any())
            throw new Exception("Some Activities weren't found");

        _appDbContext.Activities.RemoveRange(founded);
    }

    public async Task<bool> ExistIdAsync(int id, CancellationToken cancellationToken = default) => 
        await _appDbContext.Activities.FindAsync(id, cancellationToken) != null;
    
    public async Task<IEnumerable<Activity>> GetAllAsync(CancellationToken cancellationToken = default) => 
        await _appDbContext.Activities.AsNoTracking().ToListAsync(cancellationToken);
        
    
    public async Task<Activity?> GetByIdAsync(int id, CancellationToken cancellationToken = default) => 
        await _appDbContext.Activities.FindAsync(id,cancellationToken);

    public async Task<IEnumerable<Activity>> GetInternationalActivitiesByMusicianIdAsync(int id, CancellationToken cancellationToken)
    {
        var activities = await _appDbContext.MusicianActivities
                                            .Include(ma => ma.Activity)
                                            .AsNoTracking()
                                            .Where(ma => ma.MusicianId == id && ma.Activity.International)
                                            .Select(ma => ma.Activity)
                                            .ToListAsync(cancellationToken);

        return activities;
    }

    public Task<PagedResult<Activity>> SearchByFilterAsync(SearchActivitiesByFilterQuery filter, CancellationToken cancellationToken = default)
    {
        IQueryable<Activity> query = from activity in _appDbContext.Activities.AsNoTracking()
                                          select activity;

        DateTime? begin = filter.Begin;
        if (begin.HasValue)
        {
            query = query.Where(a => a.Begin >= begin.Value);
        }

        DateTime? end = filter.End;
        if(end.HasValue)
        {
            query = query.Where(a => a.End <= end.Value);
        }

        bool? international = filter.International;
        if (international.HasValue)
        {
            query = query.Where(a => a.International == international.Value);
        }

        return query.ToQuickPageList(a => a.Id, filter.Page, filter.PageSize, filter.RequestCount, cancellationToken);
    }
}
