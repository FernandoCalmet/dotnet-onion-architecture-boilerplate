using MyCompany.MyProduct.Application.Abstractions.Common;

namespace MyCompany.MyProduct.Infrastructure.Common;

internal sealed class MachineDateTime : IDateTime
{
    public DateTime UtcNow => DateTime.UtcNow;
}