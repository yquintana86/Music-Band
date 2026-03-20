using Application.Abstractions.DataContext;
using Application.Abstractions.Repositories;
using Application.PaymentDetails.Queries.SearchPaymentByFilter;
using Domain.Entities;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedLib.Models.Common;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.Intrinsics.X86;

namespace Infrastructure.Persistence.Repositories;

internal sealed class MusicianPaymentDetailsRepository : IMusicianPaymentDetailsRepository
{
    private readonly IAppDbContext _dbContext;

    public MusicianPaymentDetailsRepository(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(MusicianPaymentDetail musicianPaymentDetail, CancellationToken cancellationToken = default) =>
        _dbContext.MusicianPaymentDetails.Add(musicianPaymentDetail);

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var detail = await this._dbContext.MusicianPaymentDetails.FindAsync(id);
        ArgumentNullException.ThrowIfNull(nameof(detail));

        _dbContext.MusicianPaymentDetails.Remove(detail!);
    }

    public async Task DeleteManyAsync(List<int> ids, CancellationToken cancellationToken)
    {
        var entities = await _dbContext.MusicianPaymentDetails
                            .Where(p => ids.Contains(p.Id))
                            .ToListAsync();

        var foundIds = entities.Select(p => p.Id).ToHashSet();

        var missingIds = ids.Where(id => !foundIds.Contains(id)).ToList();
        if (missingIds.Any())
            throw new ArgumentException("Some of the items were not founded");

        _dbContext.MusicianPaymentDetails.RemoveRange(entities);
    }

    public async Task<bool> ExistIdAsync(int id, CancellationToken cancellationToken = default) =>
        await _dbContext.MusicianPaymentDetails.FindAsync(id) != null;

    public async Task<MusicianPaymentDetail?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await _dbContext.MusicianPaymentDetails.FindAsync(id);

    public async Task<IEnumerable<MusicianPaymentDetail>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _dbContext.MusicianPaymentDetails
                        .AsNoTracking()
                            .ToListAsync();


    public async Task<(decimal avg, IEnumerable<Musician> musicians)> GetAgeAvgExceedingSalary(decimal salary, CancellationToken cancellationToken)
    {
        //First option
        //var musicians = await _dbContext.MusicianPaymentDetails
        //    .Where(pd => pd.Salary > salary)
        //    .Select(pd => pd.Musician)
        //    .ToListAsync(cancellationToken);

        //var avg = musicians.Count > 0 ?
        //    (decimal)musicians.Average(m => m.Age) : 0;

        //return (avg, musicians);

        //Second option
        var response = await _dbContext.MusicianPaymentDetails
            .AsNoTracking()
            .Where(pd => pd.Salary > salary)
            .GroupBy(_ => 1)
            .Select(g => new
            {
                avg = g.Average(pd => pd.Musician.Age),
                musician = g.Select(pd => pd.Musician),
            })
            .FirstOrDefaultAsync(cancellationToken);

        return response is null ?
            (0, Enumerable.Empty<Musician>()) :
            ((decimal)response.avg, response.musician);

    }

    public Task<PagedResult<MusicianPaymentDetail>> SearchByFilterAsync(SearchPaymentDetailsByFilterQuery filter, CancellationToken cancellationToken)
    {
        IQueryable<MusicianPaymentDetail> query = from paymentDetail in _dbContext.MusicianPaymentDetails
                                                  .Include(pd => pd.Musician)
                                                  .AsNoTracking()
                                                  select paymentDetail;


        var fromPaymentDate = filter.FromPaymentDate;
        if (fromPaymentDate.HasValue)
        {
            query = query.Where(pd => pd.PaymentDate >= fromPaymentDate);
        }

        var toPaymentDate = filter.ToPaymentDate;
        if (toPaymentDate.HasValue)
        {
            query = query.Where(pd => pd.PaymentDate <= toPaymentDate);
        }

        var fromSalary = filter.FromSalary;
        if (fromSalary.HasValue)
        {
            query = query.Where(pd => pd.Salary >= fromSalary);
        }

        var toSalary = filter.ToSalary;
        if (toSalary.HasValue)
        {
            query = query.Where(pd => pd.Salary <= toSalary);
        }


        return query.ToQuickPageList(p => p.Id, filter.Page, filter.PageSize, filter.RequestCount, cancellationToken);
    }
}
