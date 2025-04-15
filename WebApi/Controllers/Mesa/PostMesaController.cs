using Domain.Common.Classes.Mesa.Create;
using Domain.Common.Classes.Mesas.Create;
using Infrastructure.Services.Mesa;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Mesa;

[ApiController]
[Route("/api/mesa")]
[Authorize]
[Tags("Mesa")]
public class PostMesaController(PostMesaHandler handler) : ControllerBase
{
    private readonly PostMesaHandler _handler = handler;

    [HttpPost]
    public async Task<ActionResult<MesaPostResponse>> Post(MesaPostRequest request)
    {
        var mesa = await _handler.Handle(request);
        return Ok(mesa);
    }
}