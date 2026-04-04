namespace Application.Abstractions.Repositories;

using Application.Musicians.Query.SearchMusiciansByFilter;
using Domain.Entities;
using SharedLib.Models.Common;
using System.Linq.Expressions;
using Musician = Domain.Entities.Musician;

public interface IMusicianRepository
{
    Task<IEnumerable<Musician>> GetAllAsync(Expression<Func<Musician, bool>> filter,  CancellationToken cancellationToken = default);
    Task<PagedResult<Musician>> SearchByFilterAsync(SearchMusicianByFilterQuery filter, CancellationToken cancellationToken = default);
    Task<bool> ExistIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Musician?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    void Add(Musician musician);
    Task DeleteAsync(int id);
    Task DeleteManyAsync(List<int> ids, CancellationToken cancellationToken);
    Task<IEnumerable<Musician>> GetMostUsedInstrumentAsync(string instrumentName, int? instrumentQtyToSearch, CancellationToken cancellationToken);
    Task<decimal> GetMusicianAverageByPlayedInstrumentsTypeAsync(IEnumerable<int> instrumentIds, CancellationToken cancellationToken);
    Task<IEnumerable<Musician>> SearchNoInternationalMusicianOlderThanAge(int age, CancellationToken cancellationToken);

    Task<Musician?> GetByIdWithRelatedEntitiesAsync<T>(int id, Expression<Func<Musician, ICollection<T>>> relatedIncluded, CancellationToken cancellationToken)
        where T : class;

    Task<MusicianSummary> GetMusicianDashboardGenericsAsync(DateTime? startDate, DateTime? endDate, CancellationToken cancellationToken);
}
