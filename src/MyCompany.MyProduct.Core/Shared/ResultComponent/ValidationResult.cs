﻿using MyCompany.MyProduct.Core.Shared.ErrorComponent;

namespace MyCompany.MyProduct.Core.Shared.ResultComponent;

public sealed class ValidationResult : Result, IValidationResult
{
    private ValidationResult(Error[] errors)
        : base(false, IValidationResult.ValidationError) =>
        Errors = errors;

    public Error[] Errors { get; }

    public static ValidationResult WithErrors(Error[] errors) => new(errors);
}