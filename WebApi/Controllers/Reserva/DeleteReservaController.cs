using Application.UseCases.Reserva.Delete;
using Infrastructure.Services.Reserva;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Reserva;

[ApiController]
[Route("/api/reserva")]
[Authorize]
[Tags("Reserva")]
public class DeleteReservaController(DeleteReservaHandler handler) : ControllerBase
{
    private readonly DeleteReservaHandler _handler = handler;
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Post(int id)
    {
        await _handler.Handle(new ReservaDeleteRequest{Id = id});
        return Created();
    }
    
}