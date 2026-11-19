using MediatR;

namespace OmniDesk.Application.Features.Users.Commands.CreateUser;

public record CreateUserCommand(
    string FullName,
    string Email,
    string Password
) : IRequest<Guid>;