using System.Reflection;
using AttribDoc.Attributes;

namespace AttribDoc.Example;

public class ExampleDocumentationGenerator : DocumentationGenerator
{
    protected override IEnumerable<MethodInfo> FindMethodsToDocument(Assembly assembly)
    {
        return assembly
            .GetTypes()
            .FirstOrDefault(t => t == typeof(ExampleApi))!
            .GetMethods()
            .Where(m => m.GetCustomAttribute<DocSummaryAttribute>() != null);
    }

    protected override void DocumentRouteHook(MethodInfo method, Route route)
    {
        Console.WriteLine("Hooked " + method.Name);
        route.Method = "GET";
        route.RouteUri = '/' + method.Name.ToLower();
    }
}