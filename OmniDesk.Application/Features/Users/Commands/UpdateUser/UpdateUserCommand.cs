using MediatR;

namespace OmniDesk.Application.Features.Users.Commands.UpdateUser;

public record UpdateUserCommand(
    Guid Id,
    string FullName,
    bool IsActive
) : IRequest<bool>;