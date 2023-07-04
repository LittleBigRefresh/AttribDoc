using System.Reflection;

namespace AttribDoc.Extensions;

internal static class MethodInfoExtensions
{
    internal static bool HasCustomAttribute<TAttribute>(this MethodInfo method) where TAttribute : Attribute 
        => method.GetCustomAttribute<TAttribute>() != null;
}