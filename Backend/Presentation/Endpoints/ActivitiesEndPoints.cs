using Application.Activities.Commands.CreateAtivity;
using Application.Activities.Commands.DeleteActivity;
using Application.Activities.Commands.UpdateActivity;
using Application.Activities.Queries.ListActivities;
using Application.Activities.Queries.SearchActivityById;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

using Presentation.Extension;

namespace Presentation.Endpoints;

public static class ActivitiesEndPoints
{

    public static void AddActivitiesEndPoints(this IEndpointRouteBuilder app)
    {
        var activityGroup = app.MapGroup("/api/activity");

        #region Commands

        activityGroup.MapPost("create", CreateActivityAsync);

        activityGroup.MapPut("update", UpdateActivityAsync);

        activityGroup.MapDelete("delete/{id:int}", DeleteActivityAsync);

        #endregion

        #region Query

        activityGroup.MapGet("list", ListActivitiesAsync);

        activityGroup.MapGet("{id:int}", SearchActivityByIdAsync);

        #endregion
    }


    #region Commands Methods
    public static async Task<IResult> CreateActivityAsync([FromBody] CreateActivityCommand command, ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(command, cancellationToken);

        return response.ToHttpResult();
    }

    public static async Task<IResult> UpdateActivityAsync([FromBody] UpdateActivityCommand command, ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(command, cancellationToken);
        return response.ToHttpResult();
    }

    public static async Task<IResult> DeleteActivityAsync(int id, ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new DeleteActivityCommand(id), cancellationToken);
        return response.ToHttpResult();
    }

    #endregion

    #region Query Methods
    public static async Task<IResult> ListActivitiesAsync(ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new SearchActivitiesQuery(), cancellationToken);
        return response.ToHttpResult();
    }

    public static async Task<IResult> SearchActivityByIdAsync(int id, ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new SearchActivityByIdQuery(id), cancellationToken);
        return response.ToHttpResult();
    }

    #endregion


}
