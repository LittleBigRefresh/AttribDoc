using System.Reflection;

namespace AttribDoc.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class DocCustomInfoAttribute : DocAttribute
{
    private readonly string _key;
    private readonly object _value;

    public DocCustomInfoAttribute(string key, object value)
    {
        _key = key;
        _value = value;
    }

    public override void AddDataToRouteDocumentation(MethodInfo method, Route route)
    {
        route.ExtraProperties.Add(this._key, this._value);
    }
}