using System.Reflection;
using AttribDoc.Attributes;

namespace AttribDoc.Example;

public class DocCustomAttribute : DocAttribute
{
    public override void AddDataToRouteDocumentation(MethodInfo method, Route route)
    {
        route.Summary += "\nCustom behavior!";
    }
}