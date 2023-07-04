using System.Reflection;

namespace AttribDoc.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class DocErrorAttribute : DocAttribute
{
    public DocErrorAttribute(Type errorType, string when)
    {
        this.ErrorType = errorType;
        this.When = when;
    }

    private Type ErrorType { get; }
    private string When { get; }
    
    public override void AddDataToRouteDocumentation(MethodInfo method, Route route)
    {
        Error error = new(this.ErrorType.Name, this.When);
        route.PotentialErrors.Add(error);
    }
}