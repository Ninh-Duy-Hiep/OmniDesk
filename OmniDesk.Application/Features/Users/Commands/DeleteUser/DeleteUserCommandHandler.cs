using MediatR;
using OmniDesk.Application.Common.Exceptions;
using OmniDesk.Application.Interfaces;
using OmniDesk.Domain.Entities;

namespace OmniDesk.Application.Features.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException(nameof(User), request.Id);
        if (user is null) return false;

        await _userRepository.DeleteAsync(user, cancellationToken);
        return true;
    }
}