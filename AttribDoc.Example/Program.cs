using System.Reflection;
using System.Text.Json;
using AttribDoc;
using AttribDoc.Example;

Console.WriteLine(ExampleApi.Add(1, 1));
Console.WriteLine(ExampleApi.GetWeather());

ExampleDocumentationGenerator generator = new();
Documentation documentation = generator.Document(Assembly.GetExecutingAssembly());

Console.WriteLine(JsonSerializer.Serialize(documentation, new JsonSerializerOptions { WriteIndented = true }));