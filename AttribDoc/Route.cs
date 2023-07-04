namespace AttribDoc;

public class Route
{
    public Route(string method, string routeUri, string summary)
    {
        this.Method = method;
        this.RouteUri = routeUri;
        this.Summary = summary;
    }

    public string Method { get; set; }
    public string RouteUri { get; set; }
    public string Summary { get; set; }
    
    public bool AuthenticationRequired { get; set; }

    public List<Parameter> Parameters { get; set; } = new();
    public List<Error> PotentialErrors { get; set; } = new();
}