using MediatR;
using Microsoft.AspNetCore.Mvc;
using OmniDesk.Application.Features.Users.Commands.CreateUser;
using OmniDesk.Application.Features.Users.Commands.DeleteUser;
using OmniDesk.Application.Features.Users.Commands.UpdateUser;
using OmniDesk.Application.Features.Users.Queries.GetAllUsers;
using OmniDesk.Application.Features.Users.Queries.GetUserByID;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ISender _sender;

    public UsersController(ISender sender) => _sender = sender;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _sender.Send(new GetAllUsersQuery(), ct));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        => Ok(await _sender.Send(new GetUserByIdQuery(id), ct));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand command, CancellationToken ct)
    {
        var userId = await _sender.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = userId }, new { Id = userId });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserCommand request, CancellationToken ct)
    {
        await _sender.Send(new UpdateUserCommand(id, request.FullName, request.IsActive), ct);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _sender.Send(new DeleteUserCommand(id), ct);
        return NoContent();
    }
}