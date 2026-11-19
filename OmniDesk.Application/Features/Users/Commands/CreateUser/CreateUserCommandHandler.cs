using MediatR;
using OmniDesk.Application.Common.Exceptions;
using OmniDesk.Application.Interfaces;
using OmniDesk.Domain.Entities;

namespace OmniDesk.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingUser is not null)
        {
            throw new ConflictException($"Email '{request.Email}' đã được sử dụng.");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName.Trim(),
            Email = request.Email.Trim().ToLowerInvariant(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user, cancellationToken);
        return user.Id;
    }
}