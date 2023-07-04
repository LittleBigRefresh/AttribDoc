using System.Reflection;

namespace AttribDoc.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class DocQueryParamAttribute : DocAttribute
{
    public DocQueryParamAttribute(string parameterName, string summary)
    {
        this.ParameterName = parameterName;
        this.Summary = summary;
    }

    private string ParameterName { get; }
    private string Summary { get; }
    
    public override void AddDataToRouteDocumentation(MethodInfo method, Route route)
    {
        route.Parameters.Add(new Parameter(this.ParameterName, ParameterType.Query, this.Summary));
    }
}