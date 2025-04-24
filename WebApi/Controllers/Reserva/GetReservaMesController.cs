using Application.UseCases.Reserva.GetAll;
using Application.UseCases.Shared;
using Infrastructure.Services.Reserva;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Reserva;

[ApiController]
[Route("/api/reserva/mes")]
[Authorize]
[Tags("Reserva")]

public class GetReservaMesController(GetReservaMesHandler handler) : ControllerBase
{
    private readonly GetReservaMesHandler _handler = handler;
    
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ReservaGetAllResponse>> Get()
    {
        var reservas = await _handler.Handle(Unit.Value);
        return Ok(reservas);
    }
}