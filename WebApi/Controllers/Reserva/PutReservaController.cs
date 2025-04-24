using Application.Interfaces.Reserva;
using Application.UseCases.Reserva.Put;
using Infrastructure.Services.Reserva;
using Infrastructure.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Reserva;

[ApiController]
[Route("/api/reserva")]
[Authorize]
[Tags("Reserva")]
public class PutReservaController(PutReservaHandler handler) : ControllerBase
{
    private readonly PutReservaHandler _handler = handler;
    
    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Put(ReservaPutRequest request)
    {
        await _handler.Handle(request);
        return Ok();
    }
}