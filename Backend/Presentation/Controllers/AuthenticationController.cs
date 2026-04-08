using Application.Authentication.Command.CreateUser;
using Application.Authentication.Command.Login;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extension;
using Infrastructure.Authentication;
using Application.Authentication.Command.RefreshToken;
using Application.Authentication.Command.Logout;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Azure;
using Application.Authentication.Command.ResetPassword;
using Application.Authentication.Command.ForgotPassword;


namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
//[ServiceFilter(typeof(ApiKeyAuthenticationFilter))]
public class AuthenticationController : ControllerBase
{

    private const string Authorization_Header = "Authorization";
    public AuthenticationController()
    {
            
    }

    [HttpPost]
    [Route("register")]
    public async Task<IResult> RegisterUserAsync([FromBody] RegisterUserCommand command, ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(command, cancellationToken);

        return response.ToHttpResult();
    }
    
    [HttpPost]
    [Route("login")]
    public async Task<IResult> LoginUserAsync([FromBody] LoginCommand request, ISender sender, CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.Email, request.Password);
        var response = await sender.Send(command, cancellationToken);

        return response.ToHttpResult();
    }
    
    [HttpPost]
    [Route("refreshtoken/{id:int}")]
    public async Task<IResult> RefreshTokenAsync([FromBody] string refreshToken, int id, ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new RefreshTokenCommand(id, refreshToken), cancellationToken);

        return response.ToHttpResult();
    }
    
    [HttpPost]
    [HasPermission(Permission.ReadUser)]
    [Route("logout")]
    public async Task<IResult> DoLogoutAsync([FromBody] string refreshToken, ISender sender, CancellationToken cancellationToken)
    {
        string? sub = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        var response = await sender.Send(new LogoutCommand(sub, refreshToken), cancellationToken);

        return response.ToHttpResult();
    }

    [HttpPost]
    [Route("resetpassword")]
    public async Task<IResult> ResetPasswordAsync([FromBody] ResetPasswordCommand resetPasswordCommand, ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(resetPasswordCommand, cancellationToken);

        return response.ToHttpResult();
    }
    [HttpPost]
    [Route("forgotpassword")]
    public async Task<IResult> ForgotPasswordAsync([FromBody] ForgotPasswordCommand resetPasswordCommand, ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(resetPasswordCommand, cancellationToken);

        return response.ToHttpResult();
    }

}
