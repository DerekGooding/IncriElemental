using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IncriElemental.Core.Engine;
using IncriElemental.Core.Models;
using IncriElemental.Desktop.Visuals;
using System;
using System.Collections.Generic;

namespace IncriElemental.Desktop.UI;

public class LayoutSystem
{
    public static void SetupButtons(List<Button> buttons, GameEngine engine, ParticleSystem particles, Action<string> logCallback)
    {
        int centerX = 512;
        int startY = 200;
        int spacing = 65;

        buttons.Add(new Button(new Rectangle(centerX - 100, startY, 200, 80), "FOCUS", Color.MediumPurple, () => {
            engine.Focus();
            particles.EmitFocus(new Vector2(centerX, startY + 40));
        }));

        startY += 100;

        buttons.Add(new Button(new Rectangle(centerX - 100, startY, 200, 50), "MANIFEST SPECK", Color.SaddleBrown, () => {
            if (engine.Manifest("speck")) logCallback("A speck of matter appears.");
        }, () => engine.State.GetResource(ResourceType.Aether).Amount >= 10 || engine.State.Discoveries.ContainsKey("first_manifestation"), "10 Aether"));

        startY += spacing;

        buttons.Add(new Button(new Rectangle(centerX - 100, startY, 200, 50), "RUNE OF ATTRACTION", Color.MediumPurple, () => {
            if (engine.Manifest("rune_of_attraction")) logCallback("The aether begins to flow of its own accord.");
        }, () => engine.State.GetResource(ResourceType.Aether).Amount >= 30 || engine.State.Discoveries.ContainsKey("automation_unlocked"), "30 Aether"));
        
        startY += spacing;

        buttons.Add(new Button(new Rectangle(centerX - 100, startY, 200, 50), "MANIFEST ALTAR", Color.Gray, () => {
            if (engine.Manifest("altar")) logCallback("A monolithic altar rises. Your capacity expands.");
        }, () => engine.State.Discoveries.ContainsKey("first_manifestation"), "100A / 20E"));

        // Column 2
        int rightX = 750;
        int rY = 200;

        buttons.Add(new Button(new Rectangle(rightX - 75, rY, 150, 50), "FORGE", Color.OrangeRed, () => {
            if (engine.Manifest("forge")) logCallback("A magical forge ignites.");
        }, () => engine.State.Discoveries.ContainsKey("altar_constructed"), "50F / 100E"));

        rY += spacing;

        buttons.Add(new Button(new Rectangle(rightX - 75, rY, 150, 50), "WELL", Color.DodgerBlue, () => {
            if (engine.Manifest("well")) logCallback("A deep Well manifests.");
        }, () => engine.State.Discoveries.ContainsKey("forge_constructed"), "300E / 100W"));

        rY += spacing;

        buttons.Add(new Button(new Rectangle(rightX - 75, rY, 150, 50), "BRAZIER", Color.OrangeRed, () => {
            if (engine.Manifest("brazier")) logCallback("A Brazier ignites.");
        }, () => engine.State.Discoveries.ContainsKey("forge_constructed"), "300E / 100F"));

        rY += spacing;

        buttons.Add(new Button(new Rectangle(rightX - 75, rY, 150, 50), "GARDEN", Color.LimeGreen, () => {
            if (engine.Manifest("garden")) logCallback("A magical Garden blooms.");
        }, () => engine.State.Discoveries.ContainsKey("forge_constructed"), "500E / 500W"));

        rY += spacing;

        buttons.Add(new Button(new Rectangle(rightX - 75, rY, 150, 50), "FAMILIAR", Color.MediumPurple, () => {
            if (engine.Manifest("familiar")) logCallback("A Familiar manifests.");
        }, () => engine.State.Discoveries.ContainsKey("garden_manifested"), "1K A / 100L"));

        // Column 3
        int farRightX = 930;
        int sY = 500;

        buttons.Add(new Button(new Rectangle(farRightX - 75, sY, 150, 45), "FOUNDATION", Color.SaddleBrown, () => {
            if (engine.Manifest("spire_foundation")) logCallback("The Spire Foundation is laid.");
        }, () => engine.State.Discoveries.ContainsKey("familiar_manifested"), "1K E / 200L"));

        sY += 55;

        buttons.Add(new Button(new Rectangle(farRightX - 75, sY, 150, 45), "SHAFT", Color.LightCyan, () => {
            if (engine.Manifest("spire_shaft")) logCallback("The Spire Shaft rises.");
        }, () => engine.State.Discoveries.ContainsKey("spire_foundation_ready"), "2K F / 1K Air"));

        sY += 55;

        buttons.Add(new Button(new Rectangle(farRightX - 75, sY, 150, 45), "CORE", Color.MediumPurple, () => {
            if (engine.Manifest("spire_core")) logCallback("The Spire Core is ignited.");
        }, () => engine.State.Discoveries.ContainsKey("spire_shaft_ready"), "5K A / 3K W"));

        buttons.Add(new Button(new Rectangle(centerX - 100, 50, 200, 80), "ASCEND", Color.Gold, () => {
            if (engine.Manifest("ascend")) logCallback("Ascension begins...");
        }, () => engine.State.Discoveries.ContainsKey("spire_complete")));
    }
}
