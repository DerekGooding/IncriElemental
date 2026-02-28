namespace IncriElemental.Core.Models;

public class GameState
{
    public int Version { get; set; } = 1;
    public Dictionary<ResourceType, Resource> Resources { get; set; } = [];
    public Dictionary<string, bool> Discoveries { get; set; } = [];
    public Dictionary<string, int> Manifestations { get; set; } = [];
    public List<Buff> ActiveBuffs { get; set; } = [];
    public WorldMap Map { get; set; } = new();

    public List<string> History { get; set; } = [];
    public double TotalGameTime { get; set; } = 0;
    public double CosmicInsight { get; set; } = 1.0;
    public bool VoidInfusionUnlocked { get; set; } = false;

    public GameState()
    {
        // Initial state
        foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
        {
            Resources[type] = new Resource(type);
        }
        Discoveries["void_observed"] = false;
        Discoveries["first_manifestation"] = false;
    }

    public Resource GetResource(ResourceType type)
    {
        if (!Resources.ContainsKey(type))
        {
            Resources[type] = new Resource(type);
        }
        return Resources[type];
    }
}
