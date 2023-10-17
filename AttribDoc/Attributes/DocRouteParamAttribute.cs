using System.Reflection;

namespace AttribDoc.Attributes;

public class DocRouteParamAttribute : DocAttribute
{
    public DocRouteParamAttribute(string parameterName, string summary)
    {
        this.ParameterName = parameterName;
        this.Summary = summary;
    }

    private string ParameterName { get; }
    private string Summary { get; }
    
    public override void AddDataToRouteDocumentation(MethodInfo method, Route route)
    {
        route.Parameters.Add(new Parameter(this.ParameterName, ParameterType.Route, this.Summary));
    }
}