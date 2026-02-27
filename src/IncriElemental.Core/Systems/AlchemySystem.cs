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
        var resA = _state.GetResource(elementA);
        var resB = _state.GetResource(elementB);

        // Mix cost: 100 of each
        if (resA.Amount < 100 || resB.Amount < 100) return false;

        resA.Add(-100);
        resB.Add(-100);

        if ((elementA == ResourceType.Fire && elementB == ResourceType.Air) ||
            (elementA == ResourceType.Air && elementB == ResourceType.Fire))
        {
            ApplyBuff("Combustion", BuffType.GenerationMultiplier, ResourceType.Aether, 2.0, 60);
            _state.History.Add("Alchemy: Combustion! Aether flows violently.");
            return true;
        }

        if ((elementA == ResourceType.Water && elementB == ResourceType.Earth) ||
            (elementA == ResourceType.Earth && elementB == ResourceType.Water))
        {
            ApplyBuff("Fertility", BuffType.GenerationMultiplier, ResourceType.Life, 3.0, 60);
            _state.History.Add("Alchemy: Fertility! The void is lush.");
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
