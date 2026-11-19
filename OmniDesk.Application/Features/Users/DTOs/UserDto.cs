namespace OmniDesk.Application.Features.Users.DTOs;

public record UserDto(
    Guid Id,
    string FullName,
    string Email,
    bool IsActive,
    DateTime CreatedAt
);