using System.Text.Json;
using IncriElemental.Core.Models;
using IncriElemental.Core.Systems;

namespace IncriElemental.Core.Engine;

public class GameEngine
{
    public GameState State { get; private set; }
    private ManifestationManager _manifestManager;
    private readonly AutomationSystem _automation;
    private readonly AlchemySystem _alchemy;
    private readonly ExplorationSystem _exploration;
    private List<LoreFragment> _lore = [];

    public GameEngine(GameState? initialState = null)
    {
        State = initialState ?? new GameState();
        _manifestManager = new ManifestationManager(State);
        _automation = new AutomationSystem(State);
        _alchemy = new AlchemySystem(State);
        _exploration = new ExplorationSystem(State);
    }

    public void LoadLore(string json) => _lore = JsonSerializer.Deserialize<List<LoreFragment>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? [];

    public void CheckLoreUnlocks(string cellType = "")
    {
        foreach (var frag in _lore)
        {
            if (State.History.Contains(frag.Text)) continue;

            var unlock = false;
            if (!string.IsNullOrEmpty(frag.UnlockDiscovery) && State.Discoveries.GetValueOrDefault(frag.UnlockDiscovery)) unlock = true;
            if (!string.IsNullOrEmpty(frag.UnlockCellType) && frag.UnlockCellType == cellType) unlock = true;

            if (unlock) State.History.Add(frag.Text);
        }
    }

    public void LoadDefinitions(string json)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        options.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        var defs = JsonSerializer.Deserialize<List<ManifestationDefinition>>(json, options);
        if (defs != null)
        {
            _manifestManager = new ManifestationManager(State, defs);
        }
    }

    public IEnumerable<ManifestationDefinition> GetDefinitions() => _manifestManager.GetDefinitions();

    public void Update(double deltaTime)
    {
        State.TotalGameTime += deltaTime;
        _alchemy.Update(deltaTime);
        _automation.Update(deltaTime, _alchemy);
        CheckLoreUnlocks();
    }

    public void Mix(ResourceType a, ResourceType b) => _alchemy.Mix(a, b);

    public bool Explore(int x, int y)
    {
        if (_exploration.Explore(x, y))
        {
            var cell = State.Map.GetCell(x, y);
            CheckLoreUnlocks(cell.Type.ToString());
            return true;
        }
        return false;
    }

    public void Focus()
    {
        var aetherGain = 1.0 * State.CosmicInsight;
        if (State.Discoveries.GetValueOrDefault("pickaxe_manifested")) aetherGain += 2.0 * State.CosmicInsight;

        State.GetResource(ResourceType.Aether).Add(aetherGain);

        if (State.GetResource(ResourceType.Aether).Amount >= 10 && !State.Discoveries.GetValueOrDefault("void_observed"))
        {
            State.Discoveries["void_observed"] = true;
            State.History.Add("The void feels thick with potential.");
        }
    }

    public bool Manifest(string thing) => _manifestManager.Manifest(thing);
}
