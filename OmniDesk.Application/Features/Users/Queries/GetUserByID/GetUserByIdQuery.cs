using MediatR;
using OmniDesk.Application.Features.Users.DTOs;

namespace OmniDesk.Application.Features.Users.Queries.GetUserByID;

public record GetUserByIdQuery(Guid Id) : IRequest<UserDto?>;