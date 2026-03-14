using Application.Activities.Commands.CreateAtivity;
using Application.Activities.Commands.DeleteActivity;
using Application.Activities.Commands.UpdateActivity;
using Application.Activities.Queries.ListActivities;
using Application.Activities.Queries.SearchActivitiesByFilter;
using Application.Activities.Queries.SearchActivityById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Presentation.Extension;

namespace Presentation.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ActivityController : ControllerBase
{
    public ActivityController()
    {
            
    }

    [HttpPost]
    [Route("create")]
    public async Task<IResult> CreateActivityAsync([FromBody] CreateActivityCommand command, ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(command, cancellationToken);

        return response.ToHttpResult();
    }

    [HttpPut]
    [Route("update")]
    public async Task<IResult> UpdateActivityAsync([FromBody] UpdateActivityCommand command, ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(command, cancellationToken);
        return response.ToHttpResult();
    }

    [HttpDelete]
    //[ServiceFilter(typeof(ApiKeyAuthenticationFilter))]
    [Route("delete/{id:int}")]
    public async Task<IResult> DeleteActivityAsync(int id, ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new DeleteActivityCommand(id), cancellationToken);
        return response.ToHttpResult();
    }


    [HttpGet]
    [Route("list")]

    public async Task<IResult> ListActivitiesAsync(ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new SearchActivitiesQuery(), cancellationToken);
        return response.ToHttpResult();
    }

    [HttpGet]
    [Route("{id:int}")]

    public async Task<IResult> SearchActivityByIdAsync(int id, ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new SearchActivityByIdQuery(id), cancellationToken);
        return response.ToHttpResult();
    }

    [HttpPost]
    [Route("search")]
    public async Task<IResult> SearchActivityByFilterAsync([FromBody] SearchActivitiesByFilterQuery filterQuery, ISender sender, CancellationToken cancellationToken)
    {
        var result = await sender.Send(filterQuery, cancellationToken);
        return result.ToHttpResult();
    }
}
