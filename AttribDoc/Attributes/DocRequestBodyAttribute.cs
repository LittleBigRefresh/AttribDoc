using System.Reflection;

namespace AttribDoc.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class DocRequestBodyAttribute : DocAttribute
{
    public DocRequestBodyAttribute(string response)
    {
        this.Object = response;
    }
    
    public DocRequestBodyAttribute(Type type)
    {
        this.Object = Activator.CreateInstance(type);
    }
    
    public DocRequestBodyAttribute(Type type, string exampleName)
    {
        FieldInfo? exampleMember = type.GetField(exampleName, BindingFlags.Public | BindingFlags.Static);
        if (exampleMember == null) throw new MissingFieldException(type.Name, exampleName);

        this.Object = exampleMember.GetValue(null);
    }

    private object Object { get; set; }
    
    public override void AddDataToRouteDocumentation(MethodInfo method, Route route)
    {
        route.ExampleRequestBody = this.Object;
    }
}