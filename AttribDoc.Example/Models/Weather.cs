namespace AttribDoc.Example.Models;

public class Weather
{
    public static Weather Example = new()
    {
        Temperature = 21,
        IsFahrenheit = false,
    };
    
    public int Temperature { get; set; }
    public bool IsFahrenheit { get; set; }
}