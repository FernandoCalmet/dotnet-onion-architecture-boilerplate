namespace MyCompany.MyProduct.Presentation.Abstractions;

public static class ApiRoutes
{
    public static class Authentication
    {
        private const string Base = "authentication";
        public const string Login = $"{Base}/login";
        public const string Register = $"{Base}/register";
    }
}