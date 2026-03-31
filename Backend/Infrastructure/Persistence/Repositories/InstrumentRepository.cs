using Application.Abstractions.DataContext;
using Application.Abstractions.Repositories;
using Application.Instruments.Queries.GetMostUsedInstruments;
using Application.Instruments.Queries.SearchInstrumentsByFilter;
using Domain.Entities;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Shared.Common;
using Shared.Models.Instrument;
using SharedLib.Models.Common;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories;

internal class InstrumentRepository : IInstrumentRepository
{
    private readonly IAppDbContext _appDbContext;


    public InstrumentRepository(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public void Add(MusicalInstrument instrument)
    {
        _appDbContext.Instruments.Add(instrument);
    }

    public async Task DeleteAsync(int id)
    {
        var instrument = await _appDbContext.Instruments.FindAsync(id);
        ArgumentNullException.ThrowIfNull(instrument);

        _appDbContext.Instruments.Remove(instrument);
    }

    public async Task DeleteManyAsync(List<int> ids)
    {
        var founded = await _appDbContext
            .Instruments
            .Where(i => ids.Contains(i.Id))
            .ToListAsync();

        var foundedHashSet = founded.Select(i => i.Id).ToHashSet();
        var diff = ids.Where(id => !foundedHashSet.Contains(id)).ToList();

        if (diff.Any())
            throw new Exception("Some instruments weren't founded");

        _appDbContext.Instruments.RemoveRange(founded);
    }

    public async Task<bool> ExistByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await _appDbContext.Instruments.FindAsync(id) != null;

    public async Task<IEnumerable<MusicalInstrument>> GetAllAsync(Expression<Func<MusicalInstrument, bool>> filter, CancellationToken cancellationToken = default) =>
        await _appDbContext.Instruments
        .Where(filter)
        .ToListAsync(cancellationToken);

    public async Task<MusicalInstrument?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await _appDbContext.Instruments.FindAsync(id);

    public async Task<Dictionary<string, IEnumerable<Musician>>> GetMostUsedInstrument(int? instrumentQtyToSearch, CancellationToken cancellationToken)
    {
        var response = await _appDbContext.Instruments
            .AsNoTracking()
            .GroupBy(i => i.Name)
            .Select(g => new
            {
                InstrumentName = g.Key,
                Musicians = g.Select(m => m.Musician).Distinct(),
                MusiciansCount = g.Select(m => m.MusicianId).Distinct().Count()
            })
            .OrderByDescending(g => g.MusiciansCount)
            .Take(instrumentQtyToSearch ?? int.MaxValue)
            .ToDictionaryAsync(d => d.InstrumentName, d => d.Musicians.AsEnumerable(),cancellationToken);

        return response;
    }

    public async Task<PagedResult<MusicalInstrument>> SearchByFilterAsync(SearchInstrumentsByFilterQuery filter, CancellationToken cancellationToken = default)
    {
        IQueryable<MusicalInstrument> query = from instrument in _appDbContext.Instruments
                                              .Include(i => i.Musician)
                                              .AsNoTracking()
                                              select instrument;


        string? name = filter.Name;
        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(i => i.Name.StartsWith(name));
        }

        string? country = filter.Country;
        if (!string.IsNullOrWhiteSpace(country))
        {
            query = query.Where(i => i.Country != null && i.Country.StartsWith(country));
        }

        InstrumentType? instrumentType = filter.Type;
        if (instrumentType.HasValue)
        {
            query = query.Where(i => i.Type == instrumentType);
        }

        return await query.ToQuickPageList(i => i.Id, filter.Page, filter.PageSize, filter.RequestCount, cancellationToken);

    }
}