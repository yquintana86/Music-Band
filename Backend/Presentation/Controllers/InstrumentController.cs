using Application.Activities.Queries.SearchActivitiesByFilter;
using Application.Instrument.Commands.CreateInstrument;
using Application.Instrument.Commands.DeleteInstrument;
using Application.Instrument.Commands.UpdateInstrument;
using Application.Instrument.Queries.GetInstruments;
using Application.Instrument.Queries.SearchInstrumentbyId;
using Application.Instruments.Commands.DeleteManyInstrument;
using Application.Instruments.Queries.SearchInstrumentsByFilter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extension;

namespace Presentation.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class InstrumentController : ControllerBase
{
    public InstrumentController()
    {

    }

    #region Queries

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IResult> SearchInstrumentAsync(int id, ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new GetMusicalInstrumentByIdQuery(id), cancellationToken);

        return response.ToHttpResult();
    }

    [HttpGet]
    [Route("list")]
    public async Task<IResult> GetInstrumentsAsync(ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new SearchInstrumentsQuery(), cancellationToken);

        return response.ToHttpResult();
    }

    [HttpPost]
    [Route("search")]
    public async Task<IResult> SearchInstrumentsByFilterAsync([FromBody] SearchInstrumentsByFilterQuery filterQuery, ISender sender, CancellationToken cancellationToken)
    {
        var result = await sender.Send(filterQuery, cancellationToken);
        return result.ToHttpResult();
    }

    #endregion

    #region Commands

    [HttpPost]
    [Route("create")]
    public async Task<IResult> CreateInstrumentAsync([FromBody] CreateInstrumentCommand command, ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(command, cancellationToken);

        return response.ToHttpResult();
    }


    [HttpPut]
    [Route("update")]
    public async Task<IResult> UpdateInstrumentAsync([FromBody] UpdateInstrumentCommand command, ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(command, cancellationToken);

        return response.ToHttpResult();
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IResult> DeleteInstrumentAsync(int id, ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new DeleteInstrumentCommand(id), cancellationToken);

        return response.ToHttpResult();
    }

    [HttpPost]
    [Route("deletemany")]
    public async Task<IResult> DeleteManyInstrumentAsync([FromBody] List<int> instrumentsIds, ISender sender,  CancellationToken cancellationToken)
    {
        var response = await sender.Send(new DeleteManyInstrumentCommand(instrumentsIds), cancellationToken);
        
        return response.ToHttpResult();
    }


    #endregion





    
    
}
