using Application.Interfaces.Reserva;
using Domain.Common.Classes.Reserva.Post;
using Domain.Common.Classes.Shared;
using Infrastructure.Services.Reserva;
using Infrastructure.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Reserva;

[ApiController]
[Route("/api/reserva")]
[Authorize]
[Tags("Reserva")]
public class PostReservaController(PostReservaHandler handler) : ControllerBase
{
    private readonly PostReservaHandler _handler = handler;
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Post(ReservaPostRequest request)
    {
        await _handler.Handle(request);
        return Created();
    }
}