using Application.Instruments.Queries.SearchInstrumentsByFilter;
using Domain.Entities;
using SharedLib.Models.Common;
using System.Linq.Expressions;

namespace Application.Abstractions.Repositories;

public interface IInstrumentRepository
{
    Task<IEnumerable<MusicalInstrument>> GetAllAsync(Expression<Func<MusicalInstrument, bool>> filter, CancellationToken cancellationToken = default);
    Task<PagedResult<MusicalInstrument>> SearchByFilterAsync(SearchInstrumentsByFilterQuery filter, CancellationToken cancellationToken = default);
    Task<MusicalInstrument?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistByIdAsync(int id, CancellationToken cancellationToken = default);
    void Add(MusicalInstrument instrument);
    Task DeleteAsync(int id);
    Task DeleteManyAsync(List<int> ids);
    Task<Dictionary<string, IEnumerable<Musician>>> GetMostUsedInstrument(int? instrumentQtyToSearch, CancellationToken cancellationToken);
}
