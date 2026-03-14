using Application.RangePlusses.Command.CreateRangePlus;
using Application.RangePlusses.Command.DeleteRangePlus;
using Application.RangePlusses.Command.UpdateRangePlus;
using Application.RangePlusses.Query.GetRangePlusById;
using Application.RangePlusses.Query.SearchRangePlusByFilter;
using Application.RangePlusses.Query.SearchRangePlusses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Presentation.Extension;

namespace Presentation.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RangePlusController : ControllerBase
{
    public RangePlusController()
    {
            
    }

    #region Commands

    [HttpPost]
    [Route("create")]
    public async Task<IResult> CreateRangePlusAsync([FromBody] CreateRangePlusCommand createRangePlusCommand, ISender sender, CancellationToken cancellationToken)
    {
        var result = await sender.Send(createRangePlusCommand, cancellationToken);

        return result.ToHttpResult();
    }

    [HttpPut]
    [Route("update")]
    public async Task<IResult> UpdateRangePlusAsync([FromBody] UpdateRangePlusCommand updateRangePlusCommand, ISender sender, CancellationToken cancellationToken)
    {
        var result = await sender.Send(updateRangePlusCommand, cancellationToken);
        return result.ToHttpResult();
    }

    [HttpDelete]
    [Route("delete/{id:int}")]
    public async Task<IResult> DeleteRangePlusAsync(int id, ISender sender, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteRangePlusCommand(id), cancellationToken);
        return result.ToHttpResult();
    }

    #endregion

    #region Queries
    [HttpGet]
    [Route("list")]
    public async Task<IResult> GetRangePlusAsync(ISender sender, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new SearchRangePlussesQuery(), cancellationToken);
        return result.ToHttpResult();
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IResult> GetRangePlusByIdAsync(int id, ISender sender, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetRangePlusByIdQuery(id), cancellationToken);
        return result.ToHttpResult();
    }

    [HttpPost]
    [Route("search")]
    public async Task<IResult> SearchRangePlusByFilterAsync([FromBody] SearchRangePlusByFilterQuery filterQuery, ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(filterQuery, cancellationToken);
        return response.ToHttpResult();
    }

    #endregion
}
