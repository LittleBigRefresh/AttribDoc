namespace AttribDoc;

[Serializable]
public class Route
{
    public string Method { get; set; }
    public string RouteUri { get; set; }
    public string Summary { get; set; }
    
    public bool AuthenticationRequired { get; set; }

    public List<Parameter> Parameters { get; set; } = new();
    public List<Error> PotentialErrors { get; set; } = new();

    public Dictionary<string, object> ExtraProperties { get; set; } = new();
    
    public object? ExampleRequestBody { get; set; }
    public object? ExampleResponse { get; set; }
}