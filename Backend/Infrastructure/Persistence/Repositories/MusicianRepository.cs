using Application.Abstractions.DataContext;
using Application.Abstractions.Repositories;
using Application.Musicians.Query.SearchMusiciansByFilter;
using Domain.Entities;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Shared.Common;
using SharedLib.Models.Common;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories;

internal class MusicianRepository : IMusicianRepository
{
    private readonly IAppDbContext _appDbContext;

    public MusicianRepository(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public void Add(Musician musician) =>
        _appDbContext.Musicians.Add(musician);

    public async Task DeleteAsync(int id)
    {
        var musician = await _appDbContext.Musicians.FirstOrDefaultAsync(m => m.Id == id);
        ArgumentNullException.ThrowIfNull(musician);

        _appDbContext.Musicians.Remove(musician);
    }

    public async Task<IEnumerable<Musician>> GetAllAsync(Expression<Func<Musician, bool>> filter, CancellationToken cancellationToken = default) =>
        await _appDbContext.Musicians
            .Where(filter)
            .ToListAsync(cancellationToken);

    public async Task<bool> ExistIdAsync(int id, CancellationToken cancellationToken = default) =>
        await _appDbContext.Musicians.FindAsync(id, cancellationToken) != null;

    public async Task<Musician?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await _appDbContext.Musicians.FindAsync(id, cancellationToken);

    public async Task<Musician?> GetByIdWithRelatedEntitiesAsync<T>(int id, Expression<Func<Musician, ICollection<T>>> relatedIncluded, CancellationToken cancellationToken)
        where T : class =>
            await _appDbContext.Musicians
                .Include(relatedIncluded)
                .FirstOrDefaultAsync(m => m.Id == id);


    public async Task<IEnumerable<Musician>> GetMostUsedInstrumentAsync(string instrumentName, int? instrumentQtyToSearch, CancellationToken cancellationToken)
    {
        var response = await _appDbContext.Instruments
            .AsNoTracking()
            .Include(i => i.Musician)
            .Where(i => EF.Functions.Like(i.Name, instrumentName + "%"))
            .Select(i => i.Musician)
            .DistinctBy(i => i.Id)
            .ToListAsync();

        return response;

        //1-Option
        //The best option for this is to let the database handle case sensitivity -> .Where(i => i.Name.StartsWith(instrumentName))
        //Then add the collation of the column/database as case insensitive -> Name NVARCHAR(100) COLLATE Latin1_General_CI_AS
        //2-.Where(i => EF.Functions.Like(i.Name, instrumentName + "%")) 
        //3-.Where(i => i.Name.StartsWith(instrumentName,StringComparison.OrdinalIgnoreCase)) 
    }

    public async Task<IEnumerable<Musician>> SearchNoInternationalMusicianOlderThanAge(int age, CancellationToken cancellationToken) =>
        await _appDbContext.Musicians
            .Where(m => m.Age > age && m.Activities.All(a => a.International))
            .ToListAsync();


    public async Task<decimal> GetMusicianAverageByPlayedInstrumentsTypeAsync(IEnumerable<int> instrumentIds, CancellationToken cancellationToken)
    {
        var total = await _appDbContext.Musicians
             .AsNoTracking()
             .Where(m =>
                m.Instruments
                .Where(i => instrumentIds.Contains(i.Id))
                .Select(i => i.Id)
                .Distinct()
                .Count() == instrumentIds.Count())
             .CountAsync(cancellationToken);

        return decimal.Round(total / (decimal)instrumentIds.Count(), 2, MidpointRounding.AwayFromZero);
    }


    public async Task<PagedResult<Musician>> SearchByFilterAsync(SearchMusicianByFilterQuery filter, CancellationToken cancellationToken = default)
    {
        IQueryable<Musician> query = from musician in _appDbContext.Musicians.AsNoTracking()
                                     select musician;


        var firstName = filter.FirstName;
        if (!string.IsNullOrWhiteSpace(firstName))
        {
            query = query.Where(m => m.FirstName.StartsWith(firstName));
        }

        var lastName = filter.LastName;
        if (!string.IsNullOrWhiteSpace(lastName))
        {
            query = query.Where(m => m.LastName.StartsWith(lastName));
        }

        var fromAge = filter.FromAge;
        if (fromAge.HasValue)
        {
            query = query.Where(m => m.Age >= fromAge);
        }

        var toAge = filter.ToAge;
        if (toAge.HasValue)
        {
            query = query.Where(m => m.Age <= toAge);
        }

        var fromExperience = filter.FromExperience;
        if (fromExperience.HasValue)
        {
            query = query.Where(m => m.Experience >= fromExperience);
        }

        var toExperience = filter.ToExperience;
        if (toExperience.HasValue)
        {
            query = query.Where(m => m.Experience <= toExperience);
        }

        var fromBasicSalary = filter.FromBasicSalary;
        if (fromBasicSalary.HasValue)
        {
            query = query.Where(m => m.BasicSalary >= fromBasicSalary);
        }

        var toBasicSalary = filter.ToBasicSalary;
        if (toBasicSalary.HasValue)
        {
            query = query.Where(m => m.BasicSalary <= toBasicSalary);
        }

        var instrumentId = filter.InstrumentId;
        if (instrumentId.HasValue && instrumentId != 0)
        {
            query = query.Where(m => m.Instruments.Any(i => i.Id == instrumentId));
        }

        return await query.ToQuickPageList(p => p.Id, filter.Page, filter.PageSize, filter.RequestCount, cancellationToken);

    }
}
