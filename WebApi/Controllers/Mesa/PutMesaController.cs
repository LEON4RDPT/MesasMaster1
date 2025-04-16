using Domain.Common.Classes.Mesa.Create;
using Domain.Common.Classes.Mesa.Put;
using Domain.Common.Classes.Mesas.Create;
using Infrastructure.Services.Mesa;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Mesa;

[ApiController]
[Route("/api/mesa")]
[Authorize]
[Tags("Mesa")]
public class PutMesaController(PutMesaHandler handler) : ControllerBase
{
    private readonly PutMesaHandler _handler = handler;

    [HttpPut]
    public async Task<IActionResult> Put(MesaPutRequest request)
    {
        var mesa = await _handler.Handle(request);
        return NoContent();
    }
}