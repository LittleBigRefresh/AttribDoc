using AttribDoc.Attributes;

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
}