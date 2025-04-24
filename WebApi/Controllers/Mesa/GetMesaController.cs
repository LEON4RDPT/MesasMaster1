using Application.UseCases.Mesa.Get;
using Infrastructure.Services.Mesa;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Mesa;

[ApiController]
[Route("/api/mesa")]
[Authorize]
[Tags("Mesa")]
public class GetMesaController(GetMesaHandler handler) : ControllerBase
{
    private readonly GetMesaHandler _handler = handler;

    [HttpGet("{id}")]
    public async Task<ActionResult<MesaGetRequest>> Get(int id)
    {
        var mesa = await _handler.Handle(new MesaGetRequest { Id = id });
        return Ok(mesa);
    }
}