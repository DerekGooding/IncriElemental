using Microsoft.Xna.Framework;
using IncriElemental.Core.Engine;
using IncriElemental.Core.Models;
using IncriElemental.Desktop.Visuals;

namespace IncriElemental.Desktop.UI;

public class LayoutSystem
{
    public static void SetupButtons(List<Button> buttons, GameEngine engine, ParticleSystem particles, AudioManager audio, Action<string> logCallback, Action<GameTab> setTab, bool aiMode = false, Action? toggleFullscreen = null)
    {
        buttons.Clear();

        // --- TAB SWITCHERS (Always visible) ---
        var tabW = 80;
        var tabH = 30;
        var centerX = UiLayout.Width / 2;
        var startX = aiMode ? centerX - 250 : centerX - 170;
        
        buttons.Add(new Button(new Rectangle(startX, 10, tabW, tabH), "VOID", Color.MediumPurple, () => setTab(GameTab.Void), tab: GameTab.None));
        buttons.Add(new Button(new Rectangle(startX + 85, 10, tabW, tabH), "SPIRE", Color.Gray, () => setTab(GameTab.Spire), tab: GameTab.None));
        buttons.Add(new Button(new Rectangle(startX + 170, 10, tabW, tabH), "WORLD", Color.LimeGreen, () => setTab(GameTab.World), tab: GameTab.None));
        buttons.Add(new Button(new Rectangle(startX + 255, 10, 120, tabH), "CONSTELLATION", Color.Gold, () => setTab(GameTab.Constellation), () => engine.State.CosmicInsight > 1.0, tab: GameTab.None));
        
        if (aiMode)
        {
            buttons.Add(new Button(new Rectangle(startX + 380, 10, 80, tabH), "DEBUG", Color.Red, () => setTab(GameTab.Debug), tab: GameTab.None));
        }

        if (toggleFullscreen != null)
        {
            buttons.Add(new Button(new Rectangle(UiLayout.Width - 110, 10, 100, 30), "FULLSCREEN", Color.Gray, toggleFullscreen, tab: GameTab.None));
        }

        // --- VOID TAB ---
        var focusBtn = new Button(UiLayout.GetCenterBounds(0.12f, 200, 80), "FOCUS", Color.MediumPurple, () => {
            engine.Focus();
            particles.EmitFocus(new Vector2(centerX, (int)(UiLayout.Height * 0.12f) + 40));
            audio.PlayFocus();
        }, tab: GameTab.Void);
        focusBtn.TooltipFunc = () => $"Gather raw magical potential.\nGain {1.0 * engine.State.CosmicInsight:F1} Aether per click.";
        buttons.Add(focusBtn);

        var defs = engine.GetDefinitions();
        var lY = 0.25f;
        var sY = 0.25f;
        var cY = 0.25f;

        foreach (var def in defs)
        {
            if (def.Id == "ascend")
            {
                var b = new Button(UiLayout.GetCenterBounds(0.06f, 200, 40), def.Name, Color.Gold, () => {
                    if (engine.Manifest(def.Id))
                    {
                        logCallback("Ascension begins...");
                        audio.PlayAscend();
                    }
                }, () => engine.State.Discoveries.GetValueOrDefault(def.RequiredDiscovery), tab: GameTab.Void);
                b.TooltipFunc = () => "Complete the Spire and transcend this reality.";
                buttons.Add(b);
                continue;
            }

            Rectangle bounds;
            GameTab tab = GameTab.Void;
            if (def.Id.Contains("spire") || def.Id.Contains("well") || def.Id.Contains("brazier") || def.Id.Contains("forge") || def.Id.Contains("clouds"))
            {
                bounds = UiLayout.GetCenterBounds(sY, 200, 50);
                sY += 0.08f;
                tab = GameTab.Spire;
            }
            else if (def.Id.Contains("garden") || def.Id.Contains("familiar"))
            {
                bounds = UiLayout.GetCenterBounds(sY, 200, 50);
                sY += 0.08f;
                tab = GameTab.World;
            }
            else if (def.Id.Contains("constellation"))
            {
                bounds = UiLayout.GetCenterBounds(cY, 200, 50);
                cY += 0.08f;
                tab = GameTab.Constellation;
            }
            else
            {
                bounds = UiLayout.GetCenterBounds(lY, 200, 50);
                lY += 0.08f;
                tab = GameTab.Void;
            }

            var btn = new Button(bounds, def.Name, GetColorForId(def.Id), () => {
                if (engine.Manifest(def.Id))
                {
                    audio.PlayManifest();
                    if (def.Id == "speck") logCallback("A speck of matter appears.");
                    else logCallback($"{def.Name} manifested.");
                }
            }, () => {
                var req = string.IsNullOrEmpty(def.RequiredDiscovery) || engine.State.Discoveries.GetValueOrDefault(def.RequiredDiscovery);
                var cost = def.Costs.All(c => engine.State.GetResource(c.Type).Amount >= c.Amount);
                var discovery = !string.IsNullOrEmpty(def.DiscoveryKey) && engine.State.Discoveries.GetValueOrDefault(def.DiscoveryKey);
                var count = engine.State.Manifestations.GetValueOrDefault(def.Id);
                return req && (cost || discovery) && count < def.MaxCount;
            }, def.Subtitle, tab: tab);
            
            btn.TooltipFunc = () => GetManifestationTooltip(def, engine);
            buttons.Add(btn);
        }

        var alchY = lY + 0.02f;
        var combBtn = new Button(UiLayout.GetCenterBounds(alchY, 200, 45), "COMBUSTION", Color.OrangeRed, () => {
            engine.Mix(ResourceType.Fire, ResourceType.Air);
            audio.PlayManifest();
        }, () => engine.State.Discoveries.ContainsKey("fire_unlocked") && engine.State.Discoveries.ContainsKey("air_unlocked"), "100F / 100Air", GameTab.Void);
        combBtn.TooltipFunc = () => "Combine Fire and Air for a temporary Aether boost.";
        buttons.Add(combBtn);

        alchY += 0.08f;
        var fertBtn = new Button(UiLayout.GetCenterBounds(alchY, 200, 45), "FERTILITY", Color.LimeGreen, () => {
            engine.Mix(ResourceType.Water, ResourceType.Earth);
            audio.PlayManifest();
        }, () => engine.State.Discoveries.ContainsKey("water_unlocked") && engine.State.Discoveries.ContainsKey("garden_manifested"), "100W / 100E", GameTab.Void);
        fertBtn.TooltipFunc = () => "Combine Water and Earth for a temporary Life boost.";
        buttons.Add(fertBtn);
    }

    private static string GetManifestationTooltip(ManifestationDefinition def, GameEngine engine)
    {
        var lines = new List<string>();
        var count = engine.State.Manifestations.GetValueOrDefault(def.Id);
        
        if (def.Effects.Any())
        {
            foreach (var effect in def.Effects)
            {
                if (effect.PerSecondBonus != 0)
                {
                    var baseVal = effect.PerSecondBonus * engine.State.CosmicInsight;
                    var totalVal = baseVal * count;
                    lines.Add($"Produces {baseVal:F1} {effect.Type}/s");
                    if (count > 0) lines.Add($"(Total: {totalVal:F1} {effect.Type}/s)");
                }
                if (effect.MaxAmountBonus != 0)
                {
                    lines.Add($"+{effect.MaxAmountBonus} {effect.Type} Storage");
                }
            }
        }

        if (def.Id == "rune_of_attraction") lines.Add("Automatically focuses the void.");
        if (def.Id == "pickaxe") lines.Add("Increases Aether gain from manual Focus.");
        if (def.Id == "forge") lines.Add("Unlocks advanced manifestation tools.");
        if (def.Id == "familiar") lines.Add("Required for world exploration.");
        if (def.Id.Contains("spire")) lines.Add("A critical component for Ascension.");

        return string.Join("\n", lines);
    }

    private static Color GetColorForId(string id)
    {
        if (id.Contains("aether") || id.Contains("attraction")) return Color.MediumPurple;
        if (id.Contains("speck") || id.Contains("foundation") || id.Contains("pickaxe")) return Color.SaddleBrown;
        if (id.Contains("spark") || id.Contains("forge") || id.Contains("brazier")) return Color.OrangeRed;
        if (id.Contains("droplet") || id.Contains("well")) return Color.DodgerBlue;
        if (id.Contains("breeze") || id.Contains("shaft") || id.Contains("clouds")) return Color.LightCyan;
        if (id.Contains("garden")) return Color.LimeGreen;
        if (id.Contains("constellation")) return Color.Gold;
        return Color.Gray;
    }
}
