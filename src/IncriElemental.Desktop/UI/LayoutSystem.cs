using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IncriElemental.Core.Engine;
using IncriElemental.Core.Models;
using IncriElemental.Core.Systems;
using IncriElemental.Desktop.Visuals;

namespace IncriElemental.Desktop.UI;

public class LayoutSystem
{
    public static void SetupButtons(List<Button> buttons, GameEngine engine, ParticleSystem particles, AudioManager audio, Action<string> logCallback, Action<GameTab> setTab, VisualManager visuals, bool aiMode = false, Action? toggleFullscreen = null)
    {
        buttons.Clear();

        var tabW = 80; var tabH = 30; var startX = 200; var centerX = UiLayout.Width / 2;
        
        buttons.Add(new Button(new Rectangle(startX, 10, tabW, tabH), TextService.Instance.Get("TAB_VOID"), Color.MediumPurple, () => setTab(GameTab.Void), tab: GameTab.None));
        buttons.Add(new Button(new Rectangle(startX + 85, 10, tabW, tabH), TextService.Instance.Get("TAB_SPIRE"), Color.Gray, () => setTab(GameTab.Spire), () => engine.State.Discoveries.ContainsKey("forge_constructed"), tab: GameTab.None));
        buttons.Add(new Button(new Rectangle(startX + 170, 10, tabW, tabH), TextService.Instance.Get("TAB_WORLD"), Color.LimeGreen, () => setTab(GameTab.World), () => engine.State.Discoveries.ContainsKey("garden_manifested"), tab: GameTab.None));
        buttons.Add(new Button(new Rectangle(startX + 255, 10, 120, tabH), TextService.Instance.Get("TAB_CONSTELLATION"), Color.Gold, () => setTab(GameTab.Constellation), () => engine.State.CosmicInsight > 1.0, tab: GameTab.None));
        buttons.Add(new Button(new Rectangle(startX + 380, 10, 80, tabH), "Flow", Color.Cyan, () => setTab(GameTab.Flow), () => engine.State.Discoveries.ContainsKey("aether_unlocked"), tab: GameTab.None));
        
        if (aiMode) buttons.Add(new Button(new Rectangle(startX + 470, 10, 80, tabH), TextService.Instance.Get("TAB_DEBUG"), Color.Red, () => setTab(GameTab.Debug), tab: GameTab.None));
        if (toggleFullscreen != null) buttons.Add(new Button(new Rectangle(100, 10, 80, 25), TextService.Instance.Get("BTN_FULLSCREEN"), Color.Gray * 0.8f, toggleFullscreen, tab: GameTab.None));

        var focusBtn = new Button(new Rectangle(centerX - 100, 0, 200, 80), TextService.Instance.Get("BTN_FOCUS"), Color.MediumPurple, () => {
            engine.Focus();
            particles.EmitFocus(new Vector2(centerX, 100));
            audio.PlayFocus();
            visuals.AddShake(2f);
        }, tab: GameTab.Void);
        focusBtn.TooltipFunc = () => TextService.Instance.Get("TOOLTIP_FOCUS", 1.0 * engine.State.CosmicInsight);
        buttons.Add(focusBtn);

        var defs = engine.GetDefinitions();
        foreach (var def in defs)
        {
            var targetTab = VisualManager.GetTabForDef(def);
            var btn = new Button(new Rectangle(centerX - 100, 0, 200, 50), def.Name, VisualManager.GetColorForId(def.Id), () => {
                if (engine.Manifest(def.Id))
                {
                    audio.PlayManifest();
                    visuals.AddShake(2f);
                    if (def.Id == "speck") logCallback(TextService.Instance.Get("HIST_SPECK_APPEARS"));
                    else if (def.Id == "ascend") logCallback(TextService.Instance.Get("HIST_ASCENSION_BEGINS"));
                    else logCallback(TextService.Instance.Get("HIST_MANIFESTED", def.Name));
                }
            }, () => {
                var req = string.IsNullOrEmpty(def.RequiredDiscovery) || engine.State.Discoveries.GetValueOrDefault(def.RequiredDiscovery);
                var cost = def.Costs.All(c => engine.State.GetResource(c.Type).Amount >= c.Amount);
                var discovery = !string.IsNullOrEmpty(def.DiscoveryKey) && engine.State.Discoveries.GetValueOrDefault(def.DiscoveryKey);
                var count = engine.State.Manifestations.GetValueOrDefault(def.Id);
                return req && (cost || discovery) && count < def.MaxCount;
            }, def.Subtitle, tab: targetTab);
            
            btn.TooltipFunc = () => visuals.GetManifestationTooltip(def, engine);
            buttons.Add(btn);
        }

        buttons.Add(new Button(new Rectangle(centerX - 100, 0, 200, 45), TextService.Instance.Get("BTN_COMBUSTION"), Color.OrangeRed, () => {
            if (engine.Mix(ResourceType.Fire, ResourceType.Air)) { audio.PlayManifest(); visuals.AddShake(3f); }
        }, () => engine.State.Discoveries.ContainsKey("fire_unlocked") && engine.State.Discoveries.ContainsKey("air_unlocked"), "100F / 100Air", GameTab.Void) { TooltipFunc = () => TextService.Instance.Get("TOOLTIP_COMBUSTION") });

        buttons.Add(new Button(new Rectangle(centerX - 100, 0, 200, 45), TextService.Instance.Get("BTN_FERTILITY"), Color.LimeGreen, () => {
            if (engine.Mix(ResourceType.Water, ResourceType.Earth)) { audio.PlayManifest(); visuals.AddShake(3f); }
        }, () => engine.State.Discoveries.ContainsKey("water_unlocked") && engine.State.Discoveries.ContainsKey("garden_manifested"), "100W / 100E", GameTab.Void) { TooltipFunc = () => TextService.Instance.Get("TOOLTIP_FERTILITY") });
    }

    public static void ApplyLayout(List<Button> buttons, GameTab currentTab)
    {
        var curY = 60; var centerX = UiLayout.Width / 2;
        foreach (var btn in buttons.Where(b => b.Tab == currentTab))
        {
            if (btn.IsVisible()) { btn.Bounds.Y = curY; btn.Bounds.X = centerX - btn.Bounds.Width / 2; curY += btn.Bounds.Height + 15; }
        }
    }

    public static void DrawFixedButtons(SpriteBatch spriteBatch, List<Button> buttons, SpriteFont font, Texture2D pixel, VisualManager visuals)
    {
        foreach (var btn in buttons) if (btn.Tab == GameTab.None && btn.IsVisible()) btn.Draw(spriteBatch, font, pixel, visuals, 0);
    }

    public static void DrawTabButtons(SpriteBatch spriteBatch, List<Button> buttons, GameTab tab, SpriteFont font, Texture2D pixel, VisualManager visuals, int scrollOffset)
    {
        foreach (var btn in buttons) if (btn.Tab == tab && btn.IsVisible()) btn.Draw(spriteBatch, font, pixel, visuals, scrollOffset);
    }

    public static void DrawTooltips(SpriteBatch sb, List<Button> buttons, GameTab tab, SpriteFont font, Texture2D pixel, VisualManager visuals, int offset, bool pinned, Button? pinnedBtn)
    {
        if (pinned && pinnedBtn != null && pinnedBtn.TooltipFunc != null) visuals.DrawTooltip(sb, font, pixel, pinnedBtn.TooltipFunc(), new Point(pinnedBtn.Bounds.Right, pinnedBtn.Bounds.Top));
        else foreach (var btn in buttons) if (btn.IsVisible() && (btn.Tab == tab || btn.Tab == GameTab.None)) btn.DrawTooltip(sb, font, pixel, visuals, (btn.Tab == GameTab.None ? 0 : offset));
    }
}
