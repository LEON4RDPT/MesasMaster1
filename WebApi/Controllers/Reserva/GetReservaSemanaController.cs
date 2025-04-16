using Domain.Common.Classes.Reserva.Get;
using Domain.Common.Classes.Reserva.GetAll;
using Domain.Common.Classes.Shared;
using Infrastructure.Services.Reserva;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Reserva;

[ApiController]
[Route("/api/reserva/semana")]
[Authorize]
[Tags("Reserva")]

public class GetReservaSemanaController(GetReservaSemanaHandler handler) : ControllerBase
{
    private readonly GetReservaSemanaHandler _handler = handler;
    
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ReservaGetAllResponse>> Get()
    {
        var reservas = await _handler.Handle(Unit.Value);
        return Ok(reservas);
    }
}