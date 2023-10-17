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

## Request/Response bodies
As of v1.1.0, you can now include request/response bodies. You can do this a couple ways:

- A normal string
```csharp
[DocSummary("Returns the string Hello World!")]
[DocResponseBody("Hello World!")]
public static string HelloWorld() => "Hello World!";
```
```json
{
  "Method": "GET",
  "RouteUri": "/helloworld",
  "Summary": "Returns the string Hello World!",
  "AuthenticationRequired": false,
  "Parameters": [],
  "PotentialErrors": [],
  "ExampleRequestBody": null,
  "ExampleResponse": "Hello World!"
}
```

- Specifying a type to be instantiated
```csharp
[DocSummary("Adds FirstNumber to SecondNumber.")]
[DocError(typeof(NotImplementedException), "A number is negative")]
[DocRequestBody(typeof(AddBody))]
public static int AddWithBody(AddBody body) => Add(body.FirstNumber, body.SecondNumber);
```
```json
{
  "Method": "GET",
  "RouteUri": "/addwithbody",
  "Summary": "Adds FirstNumber to SecondNumber.",
  "AuthenticationRequired": false,
  "Parameters": [],
  "PotentialErrors": [
    {
      "Name": "NotImplementedException",
      "OccursWhen": "A number is negative"
    }
  ],
  "ExampleRequestBody": {
    "FirstNumber": 0,
    "SecondNumber": 0
  },
  "ExampleResponse": null
}
```

- Specifying a field on a type to be used
```csharp
[DocSummary("Retrieves the current weather in your area")]
[DocResponseBody(typeof(Weather), nameof(Weather.Example))]
public static Weather GetWeather()
    => new()
    {
        Temperature = 69,
        IsFahrenheit = true
    };
```
```json
{
  "Method": "GET",
  "RouteUri": "/getweather",
  "Summary": "Retrieves the current weather in your area",
  "AuthenticationRequired": false,
  "Parameters": [],
  "PotentialErrors": [],
  "ExampleRequestBody": null,
  "ExampleResponse": {
    "Temperature": 21,
    "IsFahrenheit": false
  }
}
```

# Custom information

As of v1.2.0, you can also include objects to include in a key/value pair to be included in the object.

The easiest way is to simply use the `DocCustomInfo` attribute:

```csharp
    [DocSummary("Adds FirstNumber to SecondNumber.")]
    [DocError(typeof(NotImplementedException), "A number is negative")]
    [DocCustomInfo("test", "Custom information")]
    [DocCustomInfo("test2", 42)]
    [DocRequestBody(typeof(AddBody))]
    public static int AddWithBody(AddBody body) => Add(body.FirstNumber, body.SecondNumber);
```

You can also do this from a custom attribute:

```csharp
public class DocCustomAttribute : DocAttribute
{
    public override void AddDataToRouteDocumentation(MethodInfo method, Route route)
    {
        route.ExtraProperties.Add("Test", 42);
    }
}
```

Either method will result in something like this:

```json
{
  "Method": "GET",
  "RouteUri": "/addwithbody",
  "Summary": "Adds FirstNumber to SecondNumber.",
  "AuthenticationRequired": false,
  "Parameters": [],
  "PotentialErrors": [
    {
      "Name": "NotImplementedException",
      "OccursWhen": "A number is negative"
    }
  ],
  "ExtraProperties": {
    "test": "Custom information",
    "test2": 42
  },
  "ExampleRequestBody": {
    "FirstNumber": 0,
    "SecondNumber": 0
  },
  "ExampleResponse": null
}
```