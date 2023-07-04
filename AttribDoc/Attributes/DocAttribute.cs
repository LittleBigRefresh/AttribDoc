using System.Reflection;

namespace AttribDoc.Attributes;

public abstract class DocAttribute : Attribute
{
    public abstract void AddDataToRouteDocumentation(MethodInfo method, Route route);
}