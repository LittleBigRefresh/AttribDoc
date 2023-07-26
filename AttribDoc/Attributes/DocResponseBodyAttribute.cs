using System.Reflection;

namespace AttribDoc.Attributes;

public class DocResponseBodyAttribute : DocAttribute
{
    public DocResponseBodyAttribute(string response)
    {
        this.Object = response;
    }
    
    public DocResponseBodyAttribute(Type type)
    {
        this.Object = Activator.CreateInstance(type);
    }
    
    public DocResponseBodyAttribute(Type type, string exampleName)
    {
        FieldInfo? exampleMember = type.GetField(exampleName, BindingFlags.Public | BindingFlags.Static);
        if (exampleMember == null) throw new MissingFieldException(type.Name, exampleName);

        this.Object = exampleMember.GetValue(null);
    }

    private object Object { get; set; }
    
    public override void AddDataToRouteDocumentation(MethodInfo method, Route route)
    {
        route.ExampleResponse = this.Object;
    }
}