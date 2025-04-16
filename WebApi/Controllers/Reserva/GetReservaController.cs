using Domain.Common.Classes.Reserva.Get;
using Infrastructure.Services.Reserva;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Reserva;

[ApiController]
[Route("/api/mesa")]
[Authorize]
[Tags("Reserva")]
public class GetReservaController(GetReservaHandler handler) : ControllerBase
{
    private readonly GetReservaHandler _handler = handler;
    
    [HttpGet("{id}")]
    public async Task<ActionResult<GetReservaHandler>> Get(int id)
    {
        var reserva = await _handler.Handle(id);
        return Ok(reserva);
    }
    
}