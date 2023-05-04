namespace MyCompany.MyProduct.Presentation.Contracts.Authentication;

public sealed record RegisterUserRequest(string Email, string Password, string ConfirmPassword);