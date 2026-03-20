using Application.PaymentDetails.Queries.SearchPaymentByFilter;
using Domain.Entities;
using SharedLib.Models.Common;

namespace Application.Abstractions.Repositories;

public interface IMusicianPaymentDetailsRepository
{
    Task<IEnumerable<MusicianPaymentDetail>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PagedResult<MusicianPaymentDetail>> SearchByFilterAsync(SearchPaymentDetailsByFilterQuery filter, CancellationToken cancellationToken);
    Task<(decimal avg, IEnumerable<Musician> musicians)> GetAgeAvgExceedingSalary(decimal salary, CancellationToken cancellationToken);
    Task<MusicianPaymentDetail?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistIdAsync(int id, CancellationToken cancellationToken = default);
    void Add(MusicianPaymentDetail musicianPaymentDetail, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id,CancellationToken cancellationToken = default);
    Task DeleteManyAsync(List<int> ids, CancellationToken cancellationToken = default);
}
