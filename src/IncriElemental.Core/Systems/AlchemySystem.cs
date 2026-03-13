using IncriElemental.Core.Models;

namespace IncriElemental.Core.Systems;

public class AlchemySystem(GameState state)
{
    private readonly GameState _state = state;

    public void Update(double deltaTime)
    {
        for (var i = _state.ActiveBuffs.Count - 1; i >= 0; i--)
        {
            _state.ActiveBuffs[i].Update(deltaTime);
            if (!_state.ActiveBuffs[i].IsActive)
            {
                _state.ActiveBuffs.RemoveAt(i);
            }
        }
    }

    public bool Mix(ResourceType elementA, ResourceType elementB)
    {
        return TryMix(new Dictionary<ResourceType, double> { { elementA, 100 }, { elementB, 100 } });
    }

    public bool TryMix(Dictionary<ResourceType, double> ingredients)
    {
        foreach (var kvp in ingredients)
        {
            if (_state.GetResource(kvp.Key).Amount < kvp.Value) return false;
        }

        foreach (var kvp in ingredients) _state.GetResource(kvp.Key).Add(-kvp.Value);

        // Check for specific recipes
        if (ingredients.ContainsKey(ResourceType.Fire) && ingredients.ContainsKey(ResourceType.Air))
        {
            ApplyBuff("Combustion", BuffType.GenerationMultiplier, ResourceType.Aether, 2.0, 60);
            _state.History.Add(TextService.Instance.Get("HIST_ALCHEMY_COMBUSTION"));
            return true;
        }

        if (ingredients.ContainsKey(ResourceType.Water) && ingredients.ContainsKey(ResourceType.Earth))
        {
            ApplyBuff("Fertility", BuffType.GenerationMultiplier, ResourceType.Life, 3.0, 60);
            _state.History.Add(TextService.Instance.Get("HIST_ALCHEMY_FERTILITY"));
            return true;
        }

        return false;
    }

    private void ApplyBuff(string id, BuffType type, ResourceType target, double mult, double duration)
    {
        var existing = _state.ActiveBuffs.FirstOrDefault(b => b.Id == id);
        if (existing != null)
        {
            existing.RemainingTime = duration;
        }
        else
        {
            _state.ActiveBuffs.Add(new Buff { Id = id, Type = type, TargetResource = target, Multiplier = mult, Duration = duration, RemainingTime = duration });
        }
    }

    public double GetMultiplier(ResourceType type)
    {
        var mult = 1.0;
        foreach (var buff in _state.ActiveBuffs.Where(b => b.Type == BuffType.GenerationMultiplier && b.TargetResource == type))
        {
            mult *= buff.Multiplier;
        }
        return mult;
    }
}
