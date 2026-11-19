using MediatR;
using OmniDesk.Application.Common.Exceptions;
using OmniDesk.Application.Interfaces;
using OmniDesk.Domain.Entities;

namespace OmniDesk.Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException(nameof(User), request.Id);
        if (user is null) return false;

        user.FullName = request.FullName.Trim();
        user.IsActive = request.IsActive;
        user.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user, cancellationToken);
        return true;
    }
}