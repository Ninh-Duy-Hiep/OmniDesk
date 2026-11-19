using FluentValidation.Results;

namespace OmniDesk.Application.Common.Exceptions;

public class ValidationException : Exception
{
    public IDictionary<string, string[]> Errors { get; }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : base("Dữ liệu đầu vào không hợp lệ.")
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(group => group.Key, group => group.ToArray());
    }
}