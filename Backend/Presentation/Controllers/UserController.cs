using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extension;
using Infrastructure.Authentication;
using Application.UserApp.Queries.SearchUserById;


namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
//[ServiceFilter(typeof(ApiKeyAuthenticationFilter))]
public class UserController : ControllerBase
{
    public UserController()
    {
            
    }

    [HasPermission(Permission.ReadUser)]
    [HttpGet("{id:int}")]
    public async Task<IResult> GetUserByIdAsync(int id, ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new GetUserByIdQuery(id), cancellationToken);
        return response.ToHttpResult();
    }
}
