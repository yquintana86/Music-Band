using Application.Dashboard.Query.MusicianDashboardGenerics;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extension;

namespace Presentation.Controllers;


[Authorize]
[ApiController]
[Route("api/[controller]")]
public sealed class DashboardController : ControllerBase
{

    #region Queries

    [HttpPost]
    [Route("musiciansummary")]
    public async Task<IResult> GetMusicianDashboardGenericsAsync([FromBody] MusicianSummaryQuery query, ISender sender, CancellationToken cancellationToken)
    {
        var result = await sender.Send(query, cancellationToken);

        return result.ToHttpResult();
    }

    #endregion
}
