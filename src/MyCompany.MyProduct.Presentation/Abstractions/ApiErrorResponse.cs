using MyCompany.MyProduct.Core.Shared.ErrorComponent;

namespace MyCompany.MyProduct.Presentation.Abstractions;

public class ApiErrorResponse
{
    public ApiErrorResponse(IReadOnlyCollection<Error> errors) => Errors = errors;
    public IReadOnlyCollection<Error> Errors { get; }
}