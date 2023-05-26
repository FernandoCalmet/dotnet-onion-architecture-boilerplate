using System.Net;

namespace MyCompany.MyProduct.Application.Exceptions;

public class UnauthorizedException : CustomException
{
    public UnauthorizedException(string message)
        : base(message, null, HttpStatusCode.Unauthorized)
    {
    }
}