using MyCompany.MyProduct.Core.Shared;

namespace MyCompany.MyProduct.Core.Errors;

public static partial class DomainErrors
{
    public static class General
    {
        public static Error UnProcessableRequest => new(
            "General.UnProcessableRequest",
            "The server could not process the request.");

        public static Error ServerError => new(
            "General.ServerError",
            "The server encountered an unrecoverable error.");
    }
}