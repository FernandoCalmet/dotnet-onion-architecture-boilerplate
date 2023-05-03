using MyCompany.MyProduct.Core.Shared.ErrorComponent;

namespace MyCompany.MyProduct.Core.Shared.ResultComponent;

public interface IValidationResult
{
    public static readonly Error ValidationError = new(
        "ValidationError",
        "A validation problem occurred.");

    Error[] Errors { get; }
}