using AttribDoc.Attributes;
using AttribDoc.Example.Models;

namespace AttribDoc.Example;

public static class ExampleApi
{
    [DocSummary("Adds A to B.")]
    [DocError(typeof(NotImplementedException), "A number is negative")]
    [DocQueryParam("a", "The first number to add.")]
    [DocQueryParam("b", "The second number to add.")]
    [DocCustom]
    public static int Add(int a, int b)
    {
        if (a < 0) throw new NotImplementedException();
        if (b < 0) throw new NotImplementedException();

        return a + b;
    }

    [DocSummary("Adds FirstNumber to SecondNumber.")]
    [DocError(typeof(NotImplementedException), "A number is negative")]
    [DocRequestBody(typeof(AddBody))]
    public static int AddWithBody(AddBody body) => Add(body.FirstNumber, body.SecondNumber);

    [DocSummary("Returns the string Hello World!")]
    [DocResponseBody("Hello World!")]
    public static string HelloWorld() => "Hello World!";

    [DocSummary("Retrieves the current weather in your area")]
    [DocResponseBody(typeof(Weather), nameof(Weather.Example))]
    public static Weather GetWeather() =>
        new()
        {
            Temperature = 69,
            IsFahrenheit = true
        };
}