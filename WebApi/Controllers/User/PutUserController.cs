﻿using Application.UseCases.User.Put;
using Infrastructure.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.User;

[ApiController]
[Route("/api/user")]
[Tags("User")]
public class PutUserController(PutUserHandler handler) : ControllerBase
{
    private readonly PutUserHandler _handler = handler;

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Get(UserPutRequest request)
    {
        await _handler.Handle(request);
        return NoContent();
    }
}