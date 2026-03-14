
using Application.Musicians.Command.CreateMusician;
using Application.Musicians.Command.DeleteMusician;
using Application.Musicians.Command.UpdateMusician;
using Application.Musicians.Query.GetMusicians;
using Application.Musicians.Query.SearchMusicianById;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Presentation.Extension;

namespace Presentation.Endpoints;

public static class MusicianEndpoints
{

    public static void AddMusicianEndpoints(this IEndpointRouteBuilder app)
    {
        var musicianGroup = app.MapGroup("/api/musician");

        #region Command

        musicianGroup.MapPost("create", CreateMusicianAsync);

        musicianGroup.MapPut("update", UpdateMusicianAsync);

        musicianGroup.MapDelete("delete/{id:int}", DeleteMusicianAsync);

        #endregion


        #region Queries
        
        musicianGroup.MapGet("list", ListMusiciansAsync);

        musicianGroup.MapGet("{id:int}", SearchMusicianByIdAsync);
        #endregion
    }

    #region Command Methods

    public static async Task<IResult> CreateMusicianAsync([FromBody] CreateMusicianCommand command, ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(command, cancellationToken);

        return response.ToHttpResult();
    }



    public static async Task<IResult> UpdateMusicianAsync([FromBody] UpdateMusicianCommand command, ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(command, cancellationToken);

        return response.ToHttpResult();
    }

    public static async Task<IResult> DeleteMusicianAsync(int id, ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new DeleteMusicianCommand(id), cancellationToken);

        return response.ToHttpResult();
    }

    #endregion

    #region Queries Methods

    public static async Task<IResult> ListMusiciansAsync(ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new SearchMusiciansQuery(), cancellationToken);

        return response.ToHttpResult();
    }

    public static async Task<IResult> SearchMusicianByIdAsync(int id, ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new GetMusicianByIdQuery(id), cancellationToken);

        return response.ToHttpResult();
    }

    #endregion

}

