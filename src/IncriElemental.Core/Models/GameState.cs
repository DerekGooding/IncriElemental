namespace IncriElemental.Core.Models;

public class GameState
{
    public Dictionary<ResourceType, Resource> Resources { get; set; } = new();
    public Dictionary<string, bool> Discoveries { get; set; } = new();
    public Dictionary<string, int> Manifestations { get; set; } = new();
    public List<Buff> ActiveBuffs { get; set; } = new();
    public WorldMap Map { get; set; } = new();

    public List<string> History { get; set; } = new();
    public double TotalGameTime { get; set; } = 0;
    public double CosmicInsight { get; set; } = 1.0;

    public GameState()
    {
        // Initial state
        Resources[ResourceType.Aether] = new Resource(ResourceType.Aether);
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
