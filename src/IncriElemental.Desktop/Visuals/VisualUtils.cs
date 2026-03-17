using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IncriElemental.Core.Models;
using IncriElemental.Core.Engine;
using IncriElemental.Core.Systems;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IncriElemental.Desktop.Visuals;

public static class VisualUtils
{
    public static Color GetColorForId(string id)
    {
        if (id.Contains("aether") || id.Contains("attraction")) return Color.MediumPurple;
        if (id.Contains("speck") || id.Contains("foundation") || id.Contains("pickaxe")) return Color.SaddleBrown;
        if (id.Contains("spark") || id.Contains("forge") || id.Contains("brazier")) return Color.OrangeRed;
        if (id.Contains("droplet") || id.Contains("well")) return Color.DodgerBlue;
        if (id.Contains("breeze") || id.Contains("shaft") || id.Contains("clouds")) return Color.LightCyan;
        if (id.Contains("garden")) return Color.LimeGreen;
        return id.Contains("constellation") ? Color.Gold : Color.Gray;
    }

    public static GameTab GetTabForDef(ManifestationDefinition def)
    {
        if (def.Id.Contains("spire") || def.Id.Contains("well") || def.Id.Contains("brazier") || def.Id.Contains("forge") || def.Id.Contains("clouds")) return GameTab.Spire;
        if (def.Id.Contains("garden") || def.Id.Contains("familiar")) return GameTab.World;
        return def.Id.Contains("constellation") ? GameTab.Constellation : GameTab.Void;
    }

    public static string FormatValue(double v)
    {
        if (v >= 1_000_000_000) return $"{v / 1_000_000_000:F2}G";
        if (v >= 1_000_000) return $"{v / 1_000_000:F2}M";
        return v >= 1_000 ? $"{v / 1_000:F2}K" : v.ToString("F1");
    }

    public static string GetManifestationTooltip(ManifestationDefinition d, GameEngine e)
    {
        var l = new List<string>(); var count = e.State.Manifestations.GetValueOrDefault(d.Id);
        foreach (var ef in d.Effects)
        {
            if (ef.PerSecondBonus != 0) { 
                var b = ef.PerSecondBonus * e.State.CosmicInsight; 
                l.Add(TextService.Instance.Get("TOOLTIP_PRODUCES", b, ef.Type)); 
                if (count > 0) l.Add(TextService.Instance.Get("TOOLTIP_PRODUCES_TOTAL", b * count, ef.Type)); 
            }
            if (ef.MaxAmountBonus != 0) l.Add(TextService.Instance.Get("TOOLTIP_STORAGE", ef.MaxAmountBonus, ef.Type));
        }
        foreach (var c in d.Components) l.Add(c.GetDescription());
        if (d.Id == "rune_of_attraction") l.Add(TextService.Instance.Get("TOOLTIP_RUNE_ATTRACTION"));
        if (d.Id == "pickaxe") l.Add(TextService.Instance.Get("TOOLTIP_PICKAXE"));
        if (d.Id == "forge") l.Add(TextService.Instance.Get("TOOLTIP_FORGE"));
        if (d.Id == "familiar") l.Add(TextService.Instance.Get("TOOLTIP_FAMILIAR"));
        if (d.Id.Contains("spire")) l.Add(TextService.Instance.Get("TOOLTIP_SPIRE_PART"));
        return string.Join("\n", l);
    }

    public static void SaveScreenshot(string path, RenderTarget2D? renderTarget)
    {
        if (renderTarget == null) return;
        var dir = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);
        using var stream = File.Open(path, FileMode.Create);
        renderTarget.SaveAsPng(stream, renderTarget.Width, renderTarget.Height);
    }
}
