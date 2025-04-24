using Application.UseCases.Mesa.GetAll;
using Application.UseCases.Shared;
using Infrastructure.Services.Mesa;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Mesa;

[ApiController]
[Route("/api/mesa")]
[Authorize]
[Tags("Mesa")]
public class GetAllMesaController(GetAllMesaHandler handler) : ControllerBase
{
    private readonly GetAllMesaHandler _handler = handler;

    [HttpGet]
    public async Task<ActionResult<MesaGetAllResponse>> Get()
    {
        var mesas = await _handler.Handle(Unit.Value);
        return Ok(mesas);
    }
}