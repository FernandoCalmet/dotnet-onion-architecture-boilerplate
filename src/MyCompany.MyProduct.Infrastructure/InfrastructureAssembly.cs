using System.Reflection;

namespace MyCompany.MyProduct.Infrastructure;

public static class InfrastructureAssembly
{
    public static readonly Assembly Assembly = typeof(InfrastructureAssembly).Assembly;
}