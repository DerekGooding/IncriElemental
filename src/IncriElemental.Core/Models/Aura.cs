namespace IncriElemental.Core.Models;

public class Aura
{
    public ResourceType Type { get; set; }
    public double Intensity { get; set; }

    public Aura(ResourceType type, double intensity)
    {
        Type = type;
        Intensity = intensity;
    }
}
