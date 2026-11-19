using FluentValidation;
using OmniDesk.Application.Features.Users.Commands.UpdateUser;

namespace OmniDesk.Application.Features.Users.Validators;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id người dùng không hợp lệ.");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Họ tên không được để trống.")
            .MaximumLength(100).WithMessage("Họ tên tối đa 100 ký tự.");
    }
}