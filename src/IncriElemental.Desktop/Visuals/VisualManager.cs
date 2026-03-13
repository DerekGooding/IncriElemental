using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IncriElemental.Core.Models;
using IncriElemental.Desktop.UI;

namespace IncriElemental.Desktop.Visuals;

public class VisualManager
{
    private readonly Texture2D _pixel;
    private Effect? _bloomEffect;
    private RenderTarget2D? _renderTarget;
    public float ScreenShakeIntensity { get; private set; } = 0f;
    public float AscensionTransitionAlpha { get; private set; } = 0f;
    public float TabTransitionAlpha { get; private set; } = 0f;
    public float ReactionFlashAlpha { get; private set; } = 0f;
    public float CelebrationFlashAlpha { get; private set; } = 0f;
    private Color _reactionColor = Color.White;
    private bool _isAscending = false;
    private Color _globalTint = Color.White;
    private double _totalTime = 0;

    public VisualManager(GraphicsDevice graphicsDevice)
    {
        _pixel = new Texture2D(graphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);
        _renderTarget = new RenderTarget2D(graphicsDevice, UiLayout.Width, UiLayout.Height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
    }

    public void Resize(GraphicsDevice graphicsDevice)
    {
        _renderTarget?.Dispose();
        _renderTarget = new RenderTarget2D(graphicsDevice, UiLayout.Width, UiLayout.Height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
    }

    public void AddShake(float intensity) => ScreenShakeIntensity = Math.Max(ScreenShakeIntensity, intensity);
    public void StartTabTransition() => TabTransitionAlpha = 1.0f;
    public void StartReactionSequence(Color color) { ReactionFlashAlpha = 1.0f; _reactionColor = color; AddShake(5f); }
    public void StartCelebration() { CelebrationFlashAlpha = 1.0f; AddShake(10f); }
    public double GetTotalTime() => _totalTime;

    public void Update(float deltaTime, bool engineHasAscended, double totalProduction, ResourceType dominantResource = ResourceType.Aether)
    {
        _totalTime += deltaTime;
        if (ScreenShakeIntensity > 0) ScreenShakeIntensity = Math.Max(0, ScreenShakeIntensity - deltaTime * 5f);
        if (TabTransitionAlpha > 0) TabTransitionAlpha = Math.Max(0, TabTransitionAlpha - deltaTime * 2f);
        if (ReactionFlashAlpha > 0) ReactionFlashAlpha = Math.Max(0, ReactionFlashAlpha - deltaTime * 1.5f);
        if (CelebrationFlashAlpha > 0) CelebrationFlashAlpha = Math.Max(0, CelebrationFlashAlpha - deltaTime * 0.5f);
        if (engineHasAscended && !_isAscending) { _isAscending = true; ScreenShakeIntensity = 10f; }
        if (_isAscending && AscensionTransitionAlpha < 1.0f) AscensionTransitionAlpha += deltaTime * 0.5f;
        if (!engineHasAscended && _isAscending) { _isAscending = false; AscensionTransitionAlpha = 0f; }

        var targetColor = GetColor(dominantResource);
        var targetTint = Color.Lerp(Color.White, targetColor, 0.15f);
        _globalTint = Color.Lerp(_globalTint, targetTint, deltaTime * 0.5f);

        if (_bloomEffect != null)
        {
            float intensity = (float)(0.5 + Math.Min(2.0, Math.Log10(Math.Max(1, totalProduction)) * 0.2));
            _bloomEffect.Parameters["BloomIntensity"]?.SetValue(intensity);
            _bloomEffect.Parameters["BloomThreshold"]?.SetValue(0.4f);
        }
    }

    public Vector2 GetShakeOffset()
    {
        if (ScreenShakeIntensity <= 0) return Vector2.Zero;
        var rnd = new Random();
        return new Vector2((float)(rnd.NextDouble() * 2 - 1) * ScreenShakeIntensity, (float)(rnd.NextDouble() * 2 - 1) * ScreenShakeIntensity);
    }

    public void LoadEffects(Microsoft.Xna.Framework.Content.ContentManager content)
    {
        try { _bloomEffect = content.Load<Effect>("Bloom"); } catch { }
    }

    public void BeginRenderToTarget(GraphicsDevice graphicsDevice) => graphicsDevice.SetRenderTarget(_renderTarget);

    public void EndRenderToTarget(GraphicsDevice gd, SpriteBatch sb)
    {
        gd.SetRenderTarget(null);
        gd.Clear(Color.Black);
        sb.Begin(effect: _bloomEffect);
        sb.Draw(_renderTarget, new Rectangle(0, 0, gd.Viewport.Width, gd.Viewport.Height), _globalTint);
        sb.End();
    }

    public void Clear(GraphicsDevice gd, Color color) => gd.Clear(color);

    public void DrawOverlay(SpriteBatch sb, float alpha) { if (alpha > 0) sb.Draw(_pixel, new Rectangle(0, 0, UiLayout.Width, UiLayout.Height), Color.White * alpha); }
    public void DrawDimmer(SpriteBatch sb, float alpha) { if (alpha > 0) sb.Draw(_pixel, new Rectangle(0, 0, UiLayout.Width, UiLayout.Height), Color.Black * alpha); }
    public void DrawReactionFlash(SpriteBatch sb) { if (ReactionFlashAlpha > 0) sb.Draw(_pixel, new Rectangle(0, 0, UiLayout.Width, UiLayout.Height), _reactionColor * (ReactionFlashAlpha * 0.5f)); }
    public void DrawCelebration(SpriteBatch sb) { if (CelebrationFlashAlpha > 0) sb.Draw(_pixel, new Rectangle(0, 0, UiLayout.Width, UiLayout.Height), Color.White * CelebrationFlashAlpha); }
    public void DrawTabTransition(SpriteBatch sb) { if (TabTransitionAlpha > 0) sb.Draw(_pixel, new Rectangle(0, 0, UiLayout.Width, UiLayout.Height), Color.Black * TabTransitionAlpha); }

    public Color GetColor(ResourceType type) => ColorPalette.ElementColors.GetValueOrDefault(type, Color.White);
    public Color GetCellColor(CellType type) => ColorPalette.CellColors.GetValueOrDefault(type, Color.Black);

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

    public void DrawMap(SpriteBatch sb, WorldMap map, Point mouse, Texture2D px, int sx, int sy, GameTime gt)
    {
        var size = 20; var pad = 2; var time = gt.TotalGameTime.TotalSeconds;
        for (var x = 0; x < map.Width; x++)
        {
            for (var y = 0; y < map.Height; y++)
            {
                var cell = map.GetCell(x, y);
                var r = new Rectangle(sx + x * (size + pad), sy + y * (size + pad), size, size);
                var color = cell.IsExplored ? GetCellColor(cell.Type) : Color.DarkSlateGray * 0.3f;
                if (cell.Type == CellType.Ocean) color *= (float)Math.Sin(time * 2 + x) * 0.1f + 0.9f;
                if (cell.Type == CellType.Mountain) color *= (float)Math.Cos(time * 5 + y) * 0.05f + 0.95f;
                if (r.Contains(mouse)) color *= 1.5f;
                sb.Draw(px, r, color);
                if (cell.IsExplored && cell.Influences.Any())
                {
                    var pulse = (float)Math.Sin(time * 4) * 0.2f + 0.8f;
                    var aura = cell.Influences.OrderByDescending(a => a.Intensity).First();
                    var auraColor = GetColor(aura.Type) * (float)Math.Min(1.0, aura.Intensity) * pulse;
                    sb.Draw(px, new Rectangle(r.X - 1, r.Y - 1, r.Width + 2, 1), auraColor);
                    sb.Draw(px, new Rectangle(r.X - 1, r.Bottom, r.Width + 2, 1), auraColor);
                    sb.Draw(px, new Rectangle(r.X - 1, r.Y - 1, 1, r.Height + 2), auraColor);
                    sb.Draw(px, new Rectangle(r.Right, r.Y - 1, 1, r.Height + 2), auraColor);
                }
                if (!cell.IsExplored) { sb.Draw(px, new Rectangle(r.Left, r.Top, r.Width, 1), Color.Gray * 0.1f); sb.Draw(px, new Rectangle(r.Left, r.Bottom - 1, r.Width, 1), Color.Gray * 0.1f); sb.Draw(px, new Rectangle(r.Left, r.Top, 1, r.Height), Color.Gray * 0.1f); sb.Draw(px, new Rectangle(r.Right - 1, r.Top, 1, r.Height), Color.Gray * 0.1f); }
            }
        }
    }

    public void DrawElement(SpriteBatch sb, ResourceType t, Vector2 p, float s = 10f) => sb.Draw(_pixel, p, null, GetColor(t), 0f, new Vector2(0.5f, 0.5f), s, SpriteEffects.None, 0f);

    public void DrawSpire(SpriteBatch sb, Dictionary<string, bool> disc, double time)
    {
        if (disc.ContainsKey("spire_foundation_ready")) sb.Draw(_pixel, new Rectangle(502, 600, 20, 100), Color.Gray * 0.5f);
        if (disc.ContainsKey("spire_shaft_ready")) sb.Draw(_pixel, new Rectangle(505, 500, 14, 100), Color.LightGray * 0.5f);
        if (disc.ContainsKey("spire_complete")) { var p = (float)Math.Sin(time * 2) * 0.2f + 0.8f; sb.Draw(_pixel, new Rectangle(502, 480, 20, 20), Color.Gold * p); }
    }

    public string FormatValue(double v)
    {
        if (v >= 1_000_000_000) return $"{v / 1_000_000_000:F2}G";
        if (v >= 1_000_000) return $"{v / 1_000_000:F2}M";
        return v >= 1_000 ? $"{v / 1_000:F2}K" : v.ToString("F1");
    }

    public string GetManifestationTooltip(ManifestationDefinition d, IncriElemental.Core.Engine.GameEngine e)
    {
        var l = new List<string>(); var count = e.State.Manifestations.GetValueOrDefault(d.Id);
        foreach (var ef in d.Effects)
        {
            if (ef.PerSecondBonus != 0) { var b = ef.PerSecondBonus * e.State.CosmicInsight; l.Add(IncriElemental.Core.Systems.TextService.Instance.Get("TOOLTIP_PRODUCES", b, ef.Type)); if (count > 0) l.Add(IncriElemental.Core.Systems.TextService.Instance.Get("TOOLTIP_PRODUCES_TOTAL", b * count, ef.Type)); }
            if (ef.MaxAmountBonus != 0) l.Add(IncriElemental.Core.Systems.TextService.Instance.Get("TOOLTIP_STORAGE", ef.MaxAmountBonus, ef.Type));
        }
        foreach (var c in d.Components) l.Add(c.GetDescription());
        if (d.Id == "rune_of_attraction") l.Add(IncriElemental.Core.Systems.TextService.Instance.Get("TOOLTIP_RUNE_ATTRACTION"));
        if (d.Id == "pickaxe") l.Add(IncriElemental.Core.Systems.TextService.Instance.Get("TOOLTIP_PICKAXE"));
        if (d.Id == "forge") l.Add(IncriElemental.Core.Systems.TextService.Instance.Get("TOOLTIP_FORGE"));
        if (d.Id == "familiar") l.Add(IncriElemental.Core.Systems.TextService.Instance.Get("TOOLTIP_FAMILIAR"));
        if (d.Id.Contains("spire")) l.Add(IncriElemental.Core.Systems.TextService.Instance.Get("TOOLTIP_SPIRE_PART"));
        return string.Join("\n", l);
    }

    public void DrawTooltip(SpriteBatch sb, SpriteFont font, Texture2D px, string text, Point mouse)
    {
        if (string.IsNullOrEmpty(text)) return;
        var lines = text.Split('\n'); var parsed = lines.Select(l => RichTextSystem.Parse(l)).ToList();
        var maxWidth = parsed.Max(l => RichTextSystem.Measure(font, l, 0.8f).X); var totalH = parsed.Sum(l => RichTextSystem.Measure(font, l, 0.8f).Y) + (lines.Length - 1) * 4;
        var pos = new Vector2(mouse.X + 20, mouse.Y); if (pos.X + maxWidth > UiLayout.Width) pos.X = mouse.X - maxWidth - 20;
        var r = new Rectangle((int)pos.X - 5, (int)pos.Y - 5, (int)maxWidth + 10, (int)totalH + 10);
        sb.Draw(px, r, Color.Black * 0.9f);
        var rnd = new Random(r.X + r.Y);
        for (int i = 0; i < 5; i++)
        {
            float ox = (float)((rnd.NextDouble() * r.Width + _totalTime * 10) % r.Width);
            float oy = (float)((rnd.NextDouble() * r.Height + _totalTime * 5) % r.Height);
            sb.Draw(px, new Rectangle((int)(r.X + ox), (int)(r.Y + oy), 2, 2), Color.Gold * 0.2f);
        }
        sb.Draw(px, new Rectangle(r.X, r.Y, r.Width, 1), Color.Gray * 0.5f); sb.Draw(px, new Rectangle(r.X, r.Bottom, r.Width, 1), Color.Gray * 0.5f); sb.Draw(px, new Rectangle(r.X, r.Y, 1, r.Height), Color.Gray * 0.5f); sb.Draw(px, new Rectangle(r.Right, r.Y, 1, r.Height), Color.Gray * 0.5f);
        var curY = pos.Y; foreach (var tokens in parsed) { RichTextSystem.Draw(sb, font, tokens, new Vector2(pos.X, curY), Color.LightGoldenrodYellow, 0.8f, this); curY += font.LineSpacing * 0.8f + 4; }
    }

    public void DrawWorldElements(SpriteBatch sb, LogSystem log, SpriteFont font, Texture2D pixel, ParticleSystem particles, List<Button> buttons)
    {
        log.Draw(sb, font, pixel, this);
        particles.Draw(sb);
        LayoutSystem.DrawFixedButtons(sb, buttons, font, pixel, this);
    }

    public void DrawTooltipsAndStatus(SpriteBatch sb, List<Button> buttons, GameTab currentTab, SpriteFont font, Texture2D pixel, int curOffset, bool isPinned, Button? pinnedButton, StatusSystem status, IncriElemental.Core.Engine.GameEngine engine, int width, Point mouse)
    {
        LayoutSystem.DrawTooltips(sb, buttons, currentTab, font, pixel, this, curOffset, isPinned, pinnedButton);
        status.Draw(sb, engine, font, pixel, this, width, mouse);
    }

    public void DrawTabContent(SpriteBatch sb, GameTab tab, IncriElemental.Core.Engine.GameEngine engine, GameTime gt, MixingTableSystem mixing, Point mouse, WorldMapSystem map, SpriteFont font, Texture2D pixel, DebugSystem debug)
    {
        if (tab == GameTab.Void || tab == GameTab.Spire) DrawSpire(sb, engine.State.Discoveries, gt.TotalGameTime.TotalSeconds);
        if (tab == GameTab.Spire) mixing.Draw(sb, engine, font, pixel, this, mouse, gt);
        if (tab == GameTab.World) map.Draw(sb, engine, mouse, font, pixel, this, gt);
        if (tab == GameTab.Flow) FlowSystem.Draw(sb, engine, font, pixel, this);
        if (tab == GameTab.Debug) debug.Draw(sb, engine, font, pixel, this);
    }

    public void DrawAscended(SpriteBatch sb, EndingSystem ending, IncriElemental.Core.Engine.GameEngine engine, SpriteFont font, Texture2D pixel, GameTime gt, Point mouse, bool click, Action reset)
    {
        ending.Draw(sb, engine, font, pixel, gt, mouse, click, reset);
    }

    public void DrawPanel(SpriteBatch sb, Texture2D px, Rectangle r, Color color, float opacity = 0.1f)
    {
        sb.Draw(px, r, Color.Black * opacity);
        sb.Draw(px, r, color * (opacity * 0.5f));
        var pulse = (float)Math.Sin(_totalTime * 3.0) * 0.2f + 0.8f;
        int t = 1;
        sb.Draw(px, new Rectangle(r.X, r.Y, r.Width, t), color * (0.5f * pulse));
        sb.Draw(px, new Rectangle(r.X, r.Bottom - t, r.Width, t), color * (0.5f * pulse));
        sb.Draw(px, new Rectangle(r.X, r.Y, t, r.Height), color * (0.5f * pulse));
        sb.Draw(px, new Rectangle(r.Right - t, r.Y, t, r.Height), color * (0.5f * pulse));
        sb.Draw(px, new Rectangle(r.X, r.Y, 15, 2), color * pulse); sb.Draw(px, new Rectangle(r.X, r.Y, 2, 15), color * pulse);
        sb.Draw(px, new Rectangle(r.Right - 15, r.Y, 15, 2), color * pulse); sb.Draw(px, new Rectangle(r.Right - 2, r.Y, 2, 15), color * pulse);
        sb.Draw(px, new Rectangle(r.X, r.Bottom - 2, 15, 2), color * pulse); sb.Draw(px, new Rectangle(r.X, r.Bottom - 15, 2, 15), color * pulse);
        sb.Draw(px, new Rectangle(r.Right - 15, r.Bottom - 2, 15, 2), color * pulse); sb.Draw(px, new Rectangle(r.Right - 2, r.Bottom - 15, 2, 15), color * pulse);
        sb.Draw(px, new Rectangle(r.Center.X - 5, r.Y - 2, 10, 4), color * pulse); sb.Draw(px, new Rectangle(r.Center.X - 5, r.Bottom - 2, 10, 4), color * pulse);
    }

    public void SaveScreenshot(string path)
    {
        if (_renderTarget == null) return;
        var dir = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);
        using var stream = File.Open(path, FileMode.Create);
        _renderTarget.SaveAsPng(stream, _renderTarget.Width, _renderTarget.Height);
    }
}
