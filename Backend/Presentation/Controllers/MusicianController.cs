using Application.Musicians.Command.DeleteMusician;
using Application.Musicians.Command.UpdateMusician;
using Application.Musicians.Query.GetMusicians;
using Application.Musicians.Query.SearchMusicianById;
using Application.Musicians.Query.SearchMusiciansByFilter;
using Application.Musicians.Command.CreateMusician;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extension;
using Application.Musicians.Query.GetMusicianAverageByPlayedInstrumentsType;
using Application.Musicians.Query.SearchDomesticSeniorMusicians;

namespace Presentation.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MusicianController : ControllerBase
{
    public MusicianController()
    {

    }

    #region Command Methods


    [HttpPost]
    [Route("create")]

    public async Task<IResult> CreateMusicianAsync([FromBody] CreateMusicianCommand command, ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(command, cancellationToken);

        return response.ToHttpResult();
    }

    //[HttpPost]
    //[Route("test")]
    //public async Task<IResult> Test(string name, int nameid, [FromBody] CreateMusicianCommand command, ISender sender, CancellationToken cancellationToken)
    //{
    //    var response = await sender.Send(command, cancellationToken);

    //    return response.ToHttpResult();
    //}


    [HttpPut]
    [Route("update")]
    public async Task<IResult> UpdateMusicianAsync([FromBody] UpdateMusicianCommand command, ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(command, cancellationToken);

        return response.ToHttpResult();
    }

    [HttpDelete]
    [Route("{id:int}")]

    public async Task<IResult> DeleteMusicianAsync(int id, ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new DeleteMusicianCommand(id), cancellationToken);

        return response.ToHttpResult();
    }

    #endregion

    #region Queries Methods


    [HttpGet]
    [Route("list")]
    public async Task<IResult> ListMusiciansAsync(ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new SearchMusiciansQuery(), cancellationToken);

        return response.ToHttpResult();
    }


    [HttpGet]
    [Route("{id:int}")]

    public async Task<IResult> SearchMusicianByIdAsync(int id, ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new GetMusicianByIdQuery(id), cancellationToken);

        return response.ToHttpResult();
    }


    [HttpPost]
    [Route("search")]
    public async Task<IResult> SearchMusicianByFilterAsync([FromBody] SearchMusicianByFilterQuery filterQuery, ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(filterQuery, cancellationToken);
        return response.ToHttpResult();
    }

    [HttpPost]
    [Route("averagebyinstrument")]
    public async Task<IResult> GetMusicianAverageByInstrumentsAsync([FromBody] GetMusicianAverageByInstrumentsQuery query, ISender sender, CancellationToken cancellationToken)
    {
        var result = await sender.Send(query, cancellationToken);
        return result.ToHttpResult();
    }

    [HttpPost]
    [Route("domesticbyage")]
    public async Task<IResult> SearchDomesticSeniorMusiciansAsync([FromBody] SearchDomesticSeniorMusiciansQuery query, ISender sender, CancellationToken cancellationToken)
    {
        var result = await sender.Send(query, cancellationToken);
        return result.ToHttpResult();
    }


    #endregion

}
