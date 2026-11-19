using MediatR;
using OmniDesk.Application.Features.Users.DTOs;

namespace OmniDesk.Application.Features.Users.Queries.GetAllUsers;

public record GetAllUsersQuery : IRequest<List<UserDto>>;