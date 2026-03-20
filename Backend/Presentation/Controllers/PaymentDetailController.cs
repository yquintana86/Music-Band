using Application.PaymentDetails.Commands.CreatePaymentDetails;
using Application.PaymentDetails.Commands.DeleteManyPaymentDetails;
using Application.PaymentDetails.Commands.DeletePaymentDetail;
using Application.PaymentDetails.Commands.UpdatePaymentDetails;
using Application.PaymentDetails.Queries.GetAgeAvgExceedingSalary;
using Application.PaymentDetails.Queries.GetPaymentDetailById;
using Application.PaymentDetails.Queries.SearchPaymentByFilter;
using Application.PaymentDetails.Queries.SearchPaymentsDetails;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extension;

namespace Presentation.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PaymentDetailController : ControllerBase
{
    public PaymentDetailController()
    {

    }

    #region Commands
    [HttpPost]
    [Route("create")]
    public async Task<IResult> CreatePaymentDetailAsync([FromBody] CreatePaymentDetailCommand createPaymentDetailCommand, ISender sender, CancellationToken cancellationToken)
    {
        var result = await sender.Send(createPaymentDetailCommand, cancellationToken);
        return result.ToHttpResult();
    }

    [HttpPut]
    [Route("update")]
    public async Task<IResult> UpdatePaymentDetailAsync([FromBody] UpdatePaymentDetailCommand updatePaymentDetailCommand, ISender sender, CancellationToken cancellationToken)
    {
        var result = await sender.Send(updatePaymentDetailCommand, cancellationToken);
        return result.ToHttpResult();
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IResult> DeletePaymentDetailAsync(int id, ISender sender, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeletePaymentDetailCommand(id), cancellationToken);
        return result.ToHttpResult();
    }

    [HttpPost]
    [Route("deletemany")]
    public async Task<IResult> DeleteManyPaymentDetailAsync([FromBody] List<int> ids, ISender sender, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteManyPaymentDetailsCommand(ids), cancellationToken);
        return result.ToHttpResult();
    }

    #endregion


    #region Queries

    [HttpGet]
    [Route("list")]
    public async Task<IResult> SearchPaymentDetailsAsync(ISender sender, CancellationToken cancellationToken)
    {

        var result = await sender.Send(new SearchPaymentDetailsQuery(), cancellationToken);
        return result.ToHttpResult();
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IResult> GetPaymentDetailByIdAsync(int id, ISender sender, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetPaymentDetailByIdQuery(id), cancellationToken);
        return result.ToHttpResult();
    }

    [HttpPost]
    [Route("search")]
    public async Task<IResult> SearchPaymentDetailsByFilterAsync([FromBody] SearchPaymentDetailsByFilterQuery filterQuery, ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(filterQuery, cancellationToken);
        return response.ToHttpResult();
    }

    [HttpPost]
    [Route("ageavgexceedingsalary")]
    public async Task<IResult> GetAgeAvgExceedingSalaryQuery([FromBody] GetAgeAvgExceedingSalaryQuery getAgeAvgExceedingSalaryQuery, ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(getAgeAvgExceedingSalaryQuery, cancellationToken);
        return response.ToHttpResult();
    }

    #endregion




}
