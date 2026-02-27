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
        int spacing = 50;

        buttons.Add(new Button(new Rectangle(centerX - 100, startY, 200, 80), "FOCUS", Color.MediumPurple, () => {
            engine.Focus();
            particles.EmitFocus(new Vector2(centerX, startY + 40));
        }));

        startY += 100;

        buttons.Add(new Button(new Rectangle(centerX - 100, startY, 200, 40), "MANIFEST SPECK (10A)", Color.SaddleBrown, () => {
            if (engine.Manifest("speck")) logCallback("A speck of matter appears.");
        }, () => engine.State.GetResource(ResourceType.Aether).Amount >= 10 || engine.State.Discoveries.ContainsKey("first_manifestation")));

        startY += spacing;

        buttons.Add(new Button(new Rectangle(centerX - 100, startY, 200, 40), "RUNE OF ATTRACTION (30A)", Color.MediumPurple, () => {
            if (engine.Manifest("rune_of_attraction")) logCallback("The aether begins to flow of its own accord.");
        }, () => engine.State.GetResource(ResourceType.Aether).Amount >= 30 || engine.State.Discoveries.ContainsKey("automation_unlocked")));
        
        startY += spacing;

        buttons.Add(new Button(new Rectangle(centerX - 100, startY, 200, 40), "MANIFEST ALTAR (100A, 20E)", Color.Gray, () => {
            if (engine.Manifest("altar")) logCallback("A monolithic altar rises. Your capacity expands.");
        }, () => engine.State.Discoveries.ContainsKey("first_manifestation")));

        // Column 2 (Right Side)
        int rightX = 750;
        int rY = 200;

        buttons.Add(new Button(new Rectangle(rightX - 75, rY, 150, 40), "FORGE (50F, 100E)", Color.OrangeRed, () => {
            if (engine.Manifest("forge")) logCallback("A magical forge ignites.");
        }, () => engine.State.Discoveries.ContainsKey("altar_constructed")));

        rY += spacing;

        buttons.Add(new Button(new Rectangle(rightX - 75, rY, 150, 40), "WELL (300E, 100W)", Color.DodgerBlue, () => {
            if (engine.Manifest("well")) logCallback("A deep Well manifests.");
        }, () => engine.State.Discoveries.ContainsKey("forge_constructed")));

        rY += spacing;

        buttons.Add(new Button(new Rectangle(rightX - 75, rY, 150, 40), "BRAZIER (300E, 100F)", Color.OrangeRed, () => {
            if (engine.Manifest("brazier")) logCallback("A Brazier ignites.");
        }, () => engine.State.Discoveries.ContainsKey("forge_constructed")));

        rY += spacing;

        buttons.Add(new Button(new Rectangle(rightX - 75, rY, 150, 40), "GARDEN (500E, 500W)", Color.LimeGreen, () => {
            if (engine.Manifest("garden")) logCallback("A magical Garden blooms.");
        }, () => engine.State.Discoveries.ContainsKey("forge_constructed")));

        rY += spacing;

        buttons.Add(new Button(new Rectangle(rightX - 75, rY, 150, 40), "FAMILIAR (1000A, 100L)", Color.MediumPurple, () => {
            if (engine.Manifest("familiar")) logCallback("A Familiar manifests.");
        }, () => engine.State.Discoveries.ContainsKey("garden_manifested")));

        // Column 3 (Far Right - Spire)
        int farRightX = 930;
        int sY = 500;

        buttons.Add(new Button(new Rectangle(farRightX - 75, sY, 150, 40), "FOUNDATION", Color.SaddleBrown, () => {
            if (engine.Manifest("spire_foundation")) logCallback("The Spire Foundation is laid.");
        }, () => engine.State.Discoveries.ContainsKey("familiar_manifested")));

        sY += spacing;

        buttons.Add(new Button(new Rectangle(farRightX - 75, sY, 150, 40), "SHAFT", Color.LightCyan, () => {
            if (engine.Manifest("spire_shaft")) logCallback("The Spire Shaft rises.");
        }, () => engine.State.Discoveries.ContainsKey("spire_foundation_ready")));

        sY += spacing;

        buttons.Add(new Button(new Rectangle(farRightX - 75, sY, 150, 40), "CORE", Color.MediumPurple, () => {
            if (engine.Manifest("spire_core")) logCallback("The Spire Core is ignited.");
        }, () => engine.State.Discoveries.ContainsKey("spire_shaft_ready")));

        buttons.Add(new Button(new Rectangle(centerX - 100, 50, 200, 80), "ASCEND", Color.Gold, () => {
            if (engine.Manifest("ascend")) logCallback("Ascension begins...");
        }, () => engine.State.Discoveries.ContainsKey("spire_complete")));
    }
}
