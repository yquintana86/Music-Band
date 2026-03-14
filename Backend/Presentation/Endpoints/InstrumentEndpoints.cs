using Application.Instrument.Commands.CreateInstrument;
using Application.Instrument.Commands.DeleteInstrument;
using Application.Instrument.Commands.UpdateInstrument;
using Application.Instrument.Queries.GetInstruments;
using Application.Instrument.Queries.SearchInstrumentbyId;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Presentation.Extension;

namespace Presentation.Endpoints;

public static class InstrumentEndpoints
{
    public static void AddInstrumentEndPoints(this IEndpointRouteBuilder app)
    {
        var instrumentGroup = app.MapGroup("/api/instrument");//.AddEndpointFilter<ApiKeyAuthenticationEndPointFilter>();


        #region Commands

        instrumentGroup.MapPost("create", CreateInstrumentAsync);

        instrumentGroup.MapPut("update", UpdateInstrumentAsync);

        instrumentGroup.MapDelete("delete/{id:int}", DeleteInstrumentAsync);

        #endregion

        #region Queries

        instrumentGroup.MapGet("instrument/{id:int}", SearchInstrumentAsync);

        instrumentGroup.MapGet("instruments", GetInstrumentsAsync);

        #endregion
    }

    #region Commands Methods
    public static async Task<IResult> CreateInstrumentAsync([FromBody] CreateInstrumentCommand command, ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(command, cancellationToken);

        return response.ToHttpResult();
    }

    public static async Task<IResult> UpdateInstrumentAsync([FromBody] UpdateInstrumentCommand command, ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(command, cancellationToken);

        return response.ToHttpResult();
    }


    public static async Task<IResult> DeleteInstrumentAsync(int id, ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new DeleteInstrumentCommand(id), cancellationToken);

        return response.ToHttpResult();
    }

    #endregion

    #region Queries Methods

    public static async Task<IResult> SearchInstrumentAsync(int id, ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new GetMusicalInstrumentByIdQuery(id), cancellationToken);

        return response.ToHttpResult();
    }

    public static async Task<IResult> GetInstrumentsAsync(ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new SearchInstrumentsQuery(), cancellationToken);

        return response.ToHttpResult();
    }

    #endregion

}
