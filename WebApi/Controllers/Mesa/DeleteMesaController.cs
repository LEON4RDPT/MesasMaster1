using Application.UseCases.Mesa.Delete;
using Application.UseCases.Mesa.Get;
using Infrastructure.Services.Mesa;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Mesa;

[ApiController]
[Route("/api/mesa")]
[Authorize(Roles = "Admin")]
[Tags("Mesa")]
public class DeleteMesaController(DeleteMesaHandler handler) : ControllerBase
{
    private readonly DeleteMesaHandler _handler = handler;

    [HttpDelete("{id}")]
    public async Task<ActionResult<MesaGetRequest>> Get(int id)
    {
        await _handler.Handle(new MesaDeleteRequest{ Id = id });
        return NoContent();
    }
}