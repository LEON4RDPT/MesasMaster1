using Domain.Common.Classes.User.Delete;
using Infrastructure.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.User;

[ApiController]
[Route("/api/user")]
public class DeleteUserController(DeleteUserHandler handler) : ControllerBase
{
    private readonly DeleteUserHandler _handler = handler;

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _handler.Handle(new UserDeleteRequest { Id = id });
        return NoContent();
    }
}