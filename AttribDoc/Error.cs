namespace AttribDoc;

public class Error
{
    public Error(string name, string occursWhen)
    {
        Name = name;
        OccursWhen = occursWhen;
    }

    public string Name { get; set; }
    public string OccursWhen { get; set; }
}