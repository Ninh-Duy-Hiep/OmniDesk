using MediatR;

namespace OmniDesk.Application.Features.Users.Commands.DeleteUser;

public record DeleteUserCommand(Guid Id) : IRequest<bool>;