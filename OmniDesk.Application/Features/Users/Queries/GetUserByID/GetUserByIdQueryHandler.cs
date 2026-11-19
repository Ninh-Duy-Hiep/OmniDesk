using MediatR;
using OmniDesk.Application.Common.Exceptions;
using OmniDesk.Application.Features.Users.DTOs;
using OmniDesk.Application.Interfaces;
using OmniDesk.Domain.Entities;

namespace OmniDesk.Application.Features.Users.Queries.GetUserByID;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException(nameof(User), request.Id);

        return new UserDto(user.Id, user.FullName, user.Email, user.IsActive, user.CreatedAt);
    }
}