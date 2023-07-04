# AttribDoc

A simple library that allows you to document HTTP APIs via attributes.

## Example
```csharp
[DocSummary("Adds A to B.")]
[DocError(typeof(NotImplementedException), "A number is negative")]
[DocQueryParam("a", "The first number to add.")]
[DocQueryParam("b", "The second number to add.")]
public static int Add(int a, int b)
{
    if (a < 0) throw new NotImplementedException();
    if (b < 0) throw new NotImplementedException();
    return a + b;
}
```

```csharp
using System.Reflection;
using System.Text.Json;
using AttribDoc;
using AttribDoc.Example;

Console.WriteLine(ExampleApi.Add(1, 1));

ExampleDocumentationGenerator generator = new();
Documentation documentation = generator.Document(Assembly.GetExecutingAssembly());

Console.WriteLine(JsonSerializer.Serialize(documentation));
// Returns:
// 2
// {"Routes":[{"Method":null,"RouteUri":null,"Summary":"Adds A to B.","AuthenticationRequired":false,"Parameters":[{"Name":"a","Type":1,"Summary":"The first number to add."},{"Name":"b","Type":1,"Summary":"The second number to add."}],"PotentialErrors":[{"Name":"NotImplementedException","OccursWhen":"A number is negative"}]}]}
```

## Overriding default behavior
You can override how the documentation generator gets its information by extending `DocumentationGenerator`:
```csharp
public class ExampleDocumentationGenerator : DocumentationGenerator
{
    protected override IEnumerable<MethodInfo> FindMethodsToDocument(Assembly assembly)
    {
        return assembly
            .GetTypes()
            .FirstOrDefault(t => t == typeof(ExampleApi))!
            .GetMethods()
            .Where(m => m.GetCustomAttribute<DocSummaryAttribute>() != null);
    }

    protected override void DocumentRouteHook(MethodInfo method, Route route)
    {
        Console.WriteLine("Hooked " + method.Name);
        route.Method = "GET";
        route.RouteUri = '/' + method.Name.ToLower();
    }
}
```

Which will make the example above output:

```json lines
2
Hooked Add
{"Routes":[{"Method":"GET","RouteUri":"/add","Summary":"Adds A to B.","AuthenticationRequired":false,"Parameters":[{"Name":"a","Type":1,"Summary":"The first number to add."},{"Name":"b","Type":1,"Summary":"The second number to add."}],"PotentialErrors":[{"Name":"NotImplementedException","OccursWhen":"A number is negative"}]}]}
```

## Custom attributes

You can make an attribute extending `DocAttribute` to implement custom behavior:

```csharp
public class DocCustomAttribute : DocAttribute
{
    public override void AddDataToRouteDocumentation(MethodInfo method, Route route)
    {
        route.Summary += "\nCustom behavior!";
    }
}
```