using System.Reflection;

namespace MyCompany.MyProduct.Application;

public static class ApplicationAssembly
{
    public static readonly Assembly Assembly = typeof(ApplicationAssembly).Assembly;
}