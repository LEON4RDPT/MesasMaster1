using Application.UseCases.Shared;
using Application.UseCases.User.GetAll;
using Infrastructure.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.User;

[ApiController]
[Route("/api/user")]
[Tags("User")]
public class GetAllUserController(GetAllUserHandler handler) : ControllerBase
{
    private readonly GetAllUserHandler _handler = handler;

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserGetAllResponse>> Get()
    {
        var response = await _handler.Handle(Unit.Value);
        return Ok(response.Users);
    }
}