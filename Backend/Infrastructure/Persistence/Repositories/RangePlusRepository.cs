using Application.Abstractions.DataContext;
using Application.Abstractions.Repositories;
using Application.RangePlusses.Query.SearchRangePlusByFilter;
using Domain.Entities;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SharedLib.Models.Common;

namespace Infrastructure.Persistence.Repositories;

internal sealed class RangePlusRepository : IRangePlusRepository
{
    private readonly IAppDbContext _appDbContext;
    private readonly IMemoryCache _memoryCache;

    public RangePlusRepository(IAppDbContext appDbContext, IMemoryCache memoryCache)
    {
        _appDbContext = appDbContext;
        _memoryCache = memoryCache;
    }

    public void Add(RangePlus rangePlus, CancellationToken cancellationToken = default) => 
        _appDbContext.RangePlus.Add(rangePlus);

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var rangePlus = await _appDbContext.RangePlus.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        
        ArgumentNullException.ThrowIfNull(nameof(rangePlus));

        _appDbContext.RangePlus.Remove(rangePlus!);
    }

    public async Task<bool> ExistAsync(int id, CancellationToken cancellationToken = default) =>
        await _appDbContext.RangePlus.FindAsync(id, cancellationToken) != null;

    public async Task<IEnumerable<RangePlus>> GetAllAsync(CancellationToken cancellationToken) =>
        await _appDbContext.RangePlus.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<RangePlus?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id == 0)
            return null;

        string key = $"RangePlus-{id}";

        var range = await _memoryCache.GetOrCreateAsync(key, async entry => {
            
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));
            return await _appDbContext.RangePlus.FindAsync(id, cancellationToken);
        });


        return range;
    }

    public async Task<bool> IsExperienceRangeWithConflictAsync(int minExperience, int maxExperience, CancellationToken cancellationToken) =>
        await _appDbContext.RangePlus.AnyAsync(rp => (rp.MinExperience <= minExperience && rp.MaxExperience >= minExperience) ||
        (rp.MinExperience <= maxExperience && rp.MaxExperience >= maxExperience));

    public Task<PagedResult<RangePlus>> SearchByFilterAsync(SearchRangePlusByFilterQuery filter, CancellationToken cancellationToken)
    {
        IQueryable<RangePlus> query = from rangePlus in _appDbContext.RangePlus.AsNoTracking()
                                      select rangePlus;

        var fromPlus = filter.FromPlus;
        if (fromPlus.HasValue)
        {
            query = query.Where(rp => rp.Plus >= fromPlus);
        }

        var toPlus = filter.ToPlus;
        if (toPlus.HasValue)
        {
            query = query.Where(rp => rp.Plus <= toPlus);
        }

        var experience = filter.Experience;
        if (experience.HasValue)
        {
            query = query.Where(rp => rp.MinExperience <= experience &&
                                       rp.MaxExperience >= experience);
        }

        return query.ToQuickPageList(rp => rp.Id, filter.Page, filter.PageSize, filter.RequestCount, cancellationToken);

    }
}
