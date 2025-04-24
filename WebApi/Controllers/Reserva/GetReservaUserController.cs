using Application.UseCases.Reserva.Get;
using Application.UseCases.Reserva.GetAll;
using Infrastructure.Services.Reserva;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Reserva;

[ApiController]
[Route("/api/reserva/user")]
[Authorize]
[Tags("Reserva")]

public class GetReservaUser(GetReservaUserHandler handler) : ControllerBase
{
    private readonly GetReservaUserHandler _handler = handler;
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ReservaGetAllResponse>> Get(int id)
    {
        var reservas = await _handler.Handle(new ReservaGetRequest{ Id = id });
        return Ok(reservas);
    }
}