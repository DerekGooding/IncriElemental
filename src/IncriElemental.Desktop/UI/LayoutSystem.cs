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

        // --- FIXED UI (Always visible, no scroll) ---
        var tabW = 80;
        var tabH = 30;
        var centerX = UiLayout.Width / 2;
        var startX = aiMode ? centerX - 250 : centerX - 170;
        
        buttons.Add(new Button(new Rectangle(startX, 10, tabW, tabH), "VOID", Color.MediumPurple, () => setTab(GameTab.Void), tab: GameTab.None));
        buttons.Add(new Button(new Rectangle(startX + 85, 10, tabW, tabH), "SPIRE", Color.Gray, () => setTab(GameTab.Spire), () => engine.State.Discoveries.ContainsKey("forge_constructed"), tab: GameTab.None));
        buttons.Add(new Button(new Rectangle(startX + 170, 10, tabW, tabH), "WORLD", Color.LimeGreen, () => setTab(GameTab.World), () => engine.State.Discoveries.ContainsKey("garden_manifested"), tab: GameTab.None));
        buttons.Add(new Button(new Rectangle(startX + 255, 10, 120, tabH), "CONSTELLATION", Color.Gold, () => setTab(GameTab.Constellation), () => engine.State.CosmicInsight > 1.0, tab: GameTab.None));
        
        if (aiMode)
        {
            buttons.Add(new Button(new Rectangle(startX + 380, 10, 80, tabH), "DEBUG", Color.Red, () => setTab(GameTab.Debug), tab: GameTab.None));
        }

        if (toggleFullscreen != null)
        {
            buttons.Add(new Button(new Rectangle(100, 10, 80, 25), "FULLSCREEN", Color.Gray * 0.8f, toggleFullscreen, tab: GameTab.None));
        }

        // --- TAB CONTENT (Scrollable area) ---
        // We use Y coordinates relative to the start of the scrollable area (e.g. 100)
        // But for easier scrolling logic, we'll store them starting from 0 and offset in Draw
        
        // --- VOID TAB ---
        var focusBtn = new Button(new Rectangle(centerX - 100, 0, 200, 80), "FOCUS", Color.MediumPurple, () => {
            engine.Focus();
            particles.EmitFocus(new Vector2(centerX, 140)); // Fixed particle origin for now
            audio.PlayFocus();
        }, tab: GameTab.Void);
        focusBtn.TooltipFunc = () => $"Gather raw magical potential.\nGain {1.0 * engine.State.CosmicInsight:F1} Aether per click.";
        buttons.Add(focusBtn);

        var curY = 100; // Offset from top of tab
        var defs = engine.GetDefinitions();

        // ASCEND Button
        var ascendDef = defs.FirstOrDefault(d => d.Id == "ascend");
        if (ascendDef != null)
        {
            var b = new Button(new Rectangle(centerX - 100, curY, 200, 40), ascendDef.Name, Color.Gold, () => {
                if (engine.Manifest(ascendDef.Id))
                {
                    logCallback("Ascension begins...");
                    audio.PlayAscend();
                }
            }, () => engine.State.Discoveries.GetValueOrDefault(ascendDef.RequiredDiscovery), tab: GameTab.Void);
            b.TooltipFunc = () => "Complete the Spire and transcend this reality.";
            buttons.Add(b);
            curY += 60;
        }

        // Other Manifestations
        foreach (var def in defs.Where(d => d.Id != "ascend"))
        {
            GameTab tab = GameTab.Void;
            if (def.Id.Contains("spire") || def.Id.Contains("well") || def.Id.Contains("brazier") || def.Id.Contains("forge") || def.Id.Contains("clouds"))
                tab = GameTab.Spire;
            else if (def.Id.Contains("garden") || def.Id.Contains("familiar"))
                tab = GameTab.World;
            else if (def.Id.Contains("constellation"))
                tab = GameTab.Constellation;

            // Reset curY if tab changed? No, we need to track curY per tab.
            // Simplified: Just use a single curY and reset per tab loop if needed.
            // Better: Filter defs by tab and loop.
        }

        // Refactored loop per tab to handle dynamic stacking
        SetupTabButtons(buttons, engine, defs, GameTab.Void, centerX, 100, particles, audio, logCallback);
        SetupTabButtons(buttons, engine, defs, GameTab.Spire, centerX, 0, particles, audio, logCallback);
        SetupTabButtons(buttons, engine, defs, GameTab.World, centerX, 0, particles, audio, logCallback);
        SetupTabButtons(buttons, engine, defs, GameTab.Constellation, centerX, 0, particles, audio, logCallback);

        // --- Alchemy (Void bottom) ---
        // Find existing curY for Void
        var voidY = buttons.Where(b => b.Tab == GameTab.Void).Max(b => b.Bounds.Bottom) + 20;
        var combBtn = new Button(new Rectangle(centerX - 100, voidY, 200, 45), "COMBUSTION", Color.OrangeRed, () => {
            engine.Mix(ResourceType.Fire, ResourceType.Air);
            audio.PlayManifest();
        }, () => engine.State.Discoveries.ContainsKey("fire_unlocked") && engine.State.Discoveries.ContainsKey("air_unlocked"), "100F / 100Air", GameTab.Void);
        combBtn.TooltipFunc = () => "Combine Fire and Air for a temporary Aether boost.";
        buttons.Add(combBtn);

        var fertBtn = new Button(new Rectangle(centerX - 100, voidY + 55, 200, 45), "FERTILITY", Color.LimeGreen, () => {
            engine.Mix(ResourceType.Water, ResourceType.Earth);
            audio.PlayManifest();
        }, () => engine.State.Discoveries.ContainsKey("water_unlocked") && engine.State.Discoveries.ContainsKey("garden_manifested"), "100W / 100E", GameTab.Void);
        fertBtn.TooltipFunc = () => "Combine Water and Earth for a temporary Life boost.";
        buttons.Add(fertBtn);
    }

    private static void SetupTabButtons(List<Button> buttons, GameEngine engine, IEnumerable<ManifestationDefinition> defs, GameTab targetTab, int centerX, int startY, ParticleSystem particles, AudioManager audio, Action<string> logCallback)
    {
        var curY = startY;
        foreach (var def in defs.Where(d => d.Id != "ascend" && GetTabForDef(d) == targetTab))
        {
            var btn = new Button(new Rectangle(centerX - 100, curY, 200, 50), def.Name, GetColorForId(def.Id), () => {
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
            }, def.Subtitle, tab: targetTab);
            
            btn.TooltipFunc = () => GetManifestationTooltip(def, engine);
            buttons.Add(btn);
            curY += 65;
        }
    }

    private static GameTab GetTabForDef(ManifestationDefinition def)
    {
        if (def.Id.Contains("spire") || def.Id.Contains("well") || def.Id.Contains("brazier") || def.Id.Contains("forge") || def.Id.Contains("clouds"))
            return GameTab.Spire;
        if (def.Id.Contains("garden") || def.Id.Contains("familiar"))
            return GameTab.World;
        if (def.Id.Contains("constellation"))
            return GameTab.Constellation;
        return GameTab.Void;
    }

    private static string GetManifestationTooltip(ManifestationDefinition def, GameEngine engine)
    {
        var lines = new List<string>();
        var count = engine.State.Manifestations.GetValueOrDefault(def.Id);
        var name = string.IsNullOrEmpty(def.OutcomeName) ? def.Name : def.OutcomeName;
        
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
