using Application.UseCases.Reserva.GetAll;
using Application.UseCases.Shared;
using Infrastructure.Services.Reserva;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Reserva;

[ApiController]
[Route("/api/reserva")]
[Authorize]
[Tags("Reserva")]

public class GetAllReservaController(GetAllReservaHandler handler) : ControllerBase
{
    private readonly GetAllReservaHandler _handler = handler;
    
    [HttpGet]
    public async Task<ActionResult<ReservaGetAllResponse>> Get()
    {
        var reservas = await _handler.Handle(Unit.Value);
        return Ok(reservas);
    }
}