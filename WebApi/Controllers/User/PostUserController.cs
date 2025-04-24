using Application.UseCases.User.Create;
using Infrastructure.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.User;

[ApiController]
[Route("/api/user")]
[Tags("User")]
public class PostUserController(PostUserHandler handler) : ControllerBase
{
    private readonly PostUserHandler _handler = handler;

    [HttpPost]
    public async Task<ActionResult<UserCreateResponse>> Post(UserCreateRequest request)
    {
        var token = await _handler.Handle(request);
        return Ok(token);
    }
}