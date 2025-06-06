﻿using Application.UseCases.User.Get;
using Infrastructure.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.User;

[ApiController]
[Route("/api/user")]
[Tags("User")]
public class GetUserController(GetUserHandler handler) : ControllerBase
{
    private readonly GetUserHandler _handler = handler;

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<UserGetResponse>> Get(int id)
    {
        var user = await _handler.Handle(new UserGetRequest { Id = id });
        return Ok(user);
    }
}