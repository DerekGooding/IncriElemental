using Microsoft.Xna.Framework;
using IncriElemental.Core.Engine;
using IncriElemental.Core.Models;
using IncriElemental.Desktop.Visuals;
using System;
using System.Collections.Generic;

namespace IncriElemental.Desktop.UI;

public class LayoutSystem
{
    public static void SetupButtons(List<Button> buttons, GameEngine engine, ParticleSystem particles, AudioManager audio, Action<string> logCallback)
    {
        buttons.Clear();

        // Standard Focus Button
        buttons.Add(new Button(new Rectangle(412, 200, 200, 80), "FOCUS", Color.MediumPurple, () => {
            engine.Focus();
            particles.EmitFocus(new Vector2(512, 240));
            audio.PlayFocus();
        }));

        var defs = engine.GetDefinitions();
        var leftX = 412;
        var rightX = 675;
        var farRightX = 855;

        var lY = 300;
        var rY = 200;
        var sY = 500;

        foreach (var def in defs)
        {
            if (def.Id == "ascend")
            {
                buttons.Add(new Button(new Rectangle(412, 50, 200, 80), def.Name, Color.Gold, () => {
                    if (engine.Manifest(def.Id))
                    {
                        logCallback("Ascension begins...");
                        audio.PlayAscend();
                    }
                }, () => engine.State.Discoveries.ContainsKey(def.RequiredDiscovery)));
                continue;
            }

            // Categorize and position based on ID or discovery patterns
            Rectangle bounds;
            if (def.Id.Contains("spire"))
            {
                bounds = new Rectangle(farRightX, sY, 150, 45);
                sY += 55;
            }
            else if (def.Id.Contains("well") || def.Id.Contains("brazier") || def.Id.Contains("garden") || def.Id.Contains("familiar") || def.Id.Contains("forge"))
            {
                bounds = new Rectangle(rightX, rY, 150, 50);
                rY += 65;
            }
            else
            {
                bounds = new Rectangle(leftX, lY, 200, 50);
                lY += 65;
            }

            buttons.Add(new Button(bounds, def.Name, GetColorForId(def.Id), () => {
                if (engine.Manifest(def.Id))
                {
                    audio.PlayManifest();
                    if (def.Id == "speck") logCallback("A speck of matter appears.");
                    else logCallback($"{def.Name} manifested.");
                }
            }, () => {
                var req = string.IsNullOrEmpty(def.RequiredDiscovery) || engine.State.Discoveries.ContainsKey(def.RequiredDiscovery);
                var cost = engine.State.GetResource(def.Costs.FirstOrDefault()?.Type ?? ResourceType.Aether).Amount >= (def.Costs.FirstOrDefault()?.Amount ?? 0);
                var discovery = !string.IsNullOrEmpty(def.DiscoveryKey) && engine.State.Discoveries.ContainsKey(def.DiscoveryKey);
                return req && (cost || discovery);
            }, def.Subtitle));
        }

        // --- Alchemy (Center Column bottom) ---
        var alchY = lY + 20;
        buttons.Add(new Button(new Rectangle(412, alchY, 200, 45), "COMBUSTION", Color.OrangeRed, () => {
            engine.Mix(ResourceType.Fire, ResourceType.Air);
            audio.PlayManifest();
        }, () => engine.State.Discoveries.ContainsKey("fire_unlocked") && engine.State.Discoveries.ContainsKey("air_unlocked"), "100F / 100Air"));

        alchY += 55;
        buttons.Add(new Button(new Rectangle(412, alchY, 200, 45), "FERTILITY", Color.LimeGreen, () => {
            engine.Mix(ResourceType.Water, ResourceType.Earth);
            audio.PlayManifest();
        }, () => engine.State.Discoveries.ContainsKey("water_unlocked") && engine.State.Discoveries.ContainsKey("garden_manifested"), "100W / 100E"));
    }

    private static Color GetColorForId(string id)
    {
        if (id.Contains("aether") || id.Contains("attraction")) return Color.MediumPurple;
        if (id.Contains("speck") || id.Contains("foundation") || id.Contains("pickaxe")) return Color.SaddleBrown;
        if (id.Contains("spark") || id.Contains("forge") || id.Contains("brazier")) return Color.OrangeRed;
        if (id.Contains("droplet") || id.Contains("well")) return Color.DodgerBlue;
        if (id.Contains("breeze") || id.Contains("shaft")) return Color.LightCyan;
        if (id.Contains("garden")) return Color.LimeGreen;
        return Color.Gray;
    }
}
