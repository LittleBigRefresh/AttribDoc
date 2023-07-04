using System.Reflection;

namespace AttribDoc.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter)]
public class DocSummaryAttribute : DocAttribute
{
    public DocSummaryAttribute(string summary)
    {
        this.Summary = summary;
    }

    private string Summary { get; }
    
    public override void AddDataToRouteDocumentation(MethodInfo method, Route route)
    {
        route.Summary = this.Summary;
    }
}