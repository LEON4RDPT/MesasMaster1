using Application.UseCases.User.Auth;
using Infrastructure.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Auth;

[ApiController]
[Route("/api/login")]
[Tags("Auth")]
public class PostAuthController(PostAuthHandler handler) : ControllerBase
{
    private readonly PostAuthHandler _handler = handler;

    [HttpPost]
    public async Task<ActionResult<LoginUserResponse>> Post(LoginUserRequest request)
    {
        var response = await _handler.Handle(request);
        return Ok(response);
    }
}