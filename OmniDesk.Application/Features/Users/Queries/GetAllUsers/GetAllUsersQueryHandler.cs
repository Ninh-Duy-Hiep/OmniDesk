using MediatR;
using OmniDesk.Application.Features.Users.DTOs;
using OmniDesk.Application.Interfaces;

namespace OmniDesk.Application.Features.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserDto>>
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);

        return users.Select(u => new UserDto(
            u.Id,
            u.FullName,
            u.Email,
            u.IsActive,
            u.CreatedAt
        )).ToList();
    }
}