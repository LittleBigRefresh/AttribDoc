using System.Reflection;
using AttribDoc.Attributes;
using AttribDoc.Extensions;

namespace AttribDoc;

public class DocumentationGenerator
{
    public Documentation Document(Assembly assembly)
    {
        Documentation documentation = new();
        
        List<MethodInfo> methods = FindMethodsToDocument(assembly).ToList();
        IEnumerable<Route> routes = this.DocumentRoutes(methods);
        documentation.Routes = routes.ToList();

        return documentation;
    }
    
    /// <summary>
    /// Finds a list of methods to generate documentation for. By default, scans for methods with DocSummary.
    /// </summary>
    /// <param name="assembly">The assembly to find.</param>
    protected virtual IEnumerable<MethodInfo> FindMethodsToDocument(Assembly assembly)
    {
        return assembly.GetTypes()
            .SelectMany(t => t.GetMethods())
            .Where(m => m.HasCustomAttribute<DocSummaryAttribute>());
    }

    /// <summary>
    /// Called before getting data from attributes.
    /// </summary>
    /// <param name="method">The assembly information of the method being processed.</param>
    /// <param name="route">The route being processed.</param>
    protected virtual void DocumentRouteHook(MethodInfo method, Route route)
    {
        // In case the user needs to perform extra steps unable to be done with attributes, we provide a hook
    }

    private IEnumerable<Route> DocumentRoutes(ICollection<MethodInfo> methods)
    {
        List<Route> routes = new List<Route>(methods.Count);
        routes.AddRange(methods.Select(DocumentRoute));

        return routes;
    }

    private Route DocumentRoute(MethodInfo method)
    {
        Route route = new();
        this.DocumentRouteHook(method, route);

        foreach (DocAttribute attribute in method.GetCustomAttributes<DocAttribute>())
        {
            attribute.AddDataToRouteDocumentation(method, route);
        }
        
        foreach (ParameterInfo param in method.GetParameters())
        {
            DocSummaryAttribute? attribute = param.GetCustomAttribute<DocSummaryAttribute>();
            if(attribute == null) continue;
            
            route.Parameters.Add(new Parameter(param.Name, ParameterType.Route, attribute.Summary));
        }

        return route;
    }
}