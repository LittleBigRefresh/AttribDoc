using System.Reflection;

namespace AttribDoc.Example;

public class ExampleDocumentationGenerator : DocumentationGenerator
{
    protected override void DocumentRouteHook(MethodInfo method, Route route)
    {
        Console.WriteLine("Hooked " + method.Name);
        route.Method = "GET";
        route.RouteUri = '/' + method.Name.ToLower();
    }
}