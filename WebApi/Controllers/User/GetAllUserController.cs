using Application.Exceptions;
using Application.Exceptions.User;
using Domain.Common.Classes;
using Domain.Common.Classes.User.Get;
using Domain.Common.Classes.User.GetAll;
using Infrastructure.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.User;

[ApiController]
[Route("/api/user")]
public class GetAllUserController(GetAllUserHandler handler) : ControllerBase
{
    private readonly GetAllUserHandler _handler = handler;
    
    [HttpGet]
    public async Task<ActionResult<UserGetAllResponse>> Get()
    {
        var response = await _handler.Handle(Unit.Value);
        return Ok(response.Users);
    }
    
   
}