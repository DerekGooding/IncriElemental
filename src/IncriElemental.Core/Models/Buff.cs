using System;

namespace IncriElemental.Core.Models;

public enum BuffType
{
    GenerationMultiplier,
    ManualFocusBonus,
    CostReduction
}

public class Buff
{
    public string Id { get; set; } = string.Empty;
    public BuffType Type { get; set; }
    public double Multiplier { get; set; } = 1.0;
    public double Duration { get; set; } // in seconds
    public double RemainingTime { get; set; }
    public ResourceType TargetResource { get; set; }

    public bool IsActive => RemainingTime > 0;

    public void Update(double deltaTime)
    {
        RemainingTime = Math.Max(0, RemainingTime - deltaTime);
    }
}
