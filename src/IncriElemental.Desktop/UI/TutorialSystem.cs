using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IncriElemental.Core.Engine;
using IncriElemental.Core.Models;
using IncriElemental.Core.Systems;

namespace IncriElemental.Desktop.UI;

public enum TutorialStep
{
    None,
    Focus,
    ManifestFirst,
    Automate,
    Complete
}

public class TutorialSystem
{
    public TutorialStep CurrentStep { get; private set; } = TutorialStep.None;
    private bool _isActive = false;

    public void Start(GameState state)
    {
        if (state.TotalGameTime < 10 && state.Manifestations.Count == 0)
        {
            _isActive = true;
            CurrentStep = TutorialStep.Focus;
        }
    }

    public void Update(GameState state)
    {
        if (!_isActive) return;

        switch (CurrentStep)
        {
            case TutorialStep.Focus:
                if (state.GetResource(Models.ResourceType.Aether).Amount >= 10)
                    CurrentStep = TutorialStep.ManifestFirst;
                break;
            case TutorialStep.ManifestFirst:
                if (state.Manifestations.Count > 0)
                    CurrentStep = TutorialStep.Automate;
                break;
            case TutorialStep.Automate:
                if (state.Resources.Values.Any(r => r.PerSecond > 0))
                    CurrentStep = TutorialStep.Complete;
                break;
            case TutorialStep.Complete:
                _isActive = false;
                break;
        }
    }

    public void Draw(SpriteBatch spriteBatch, SpriteFont font, Texture2D pixel, List<Button> buttons)
    {
        if (!_isActive) return;

        // Dim background
        spriteBatch.Draw(pixel, new Rectangle(0, 0, UiLayout.Width, UiLayout.Height), Color.Black * 0.5f);

        string titleKey = $"TUT_TITLE_{CurrentStep.ToString().ToUpper()}";
        string descKey = $"TUT_DESC_{CurrentStep.ToString().ToUpper()}";

        var title = TextService.Instance.Get(titleKey);
        var desc = TextService.Instance.Get(descKey);

        var titleSize = font.MeasureString(title);
        var descSize = font.MeasureString(desc) * 0.8f;

        var boxW = 400;
        var boxH = 120;
        var boxRect = new Rectangle(UiLayout.Width / 2 - boxW / 2, 150, boxW, boxH);

        spriteBatch.Draw(pixel, boxRect, Color.Black * 0.8f);
        spriteBatch.Draw(pixel, new Rectangle(boxRect.X, boxRect.Y, boxRect.Width, 1), Color.MediumPurple);
        spriteBatch.Draw(pixel, new Rectangle(boxRect.X, boxRect.Bottom, boxRect.Width, 1), Color.MediumPurple);

        spriteBatch.DrawString(font, title, new Vector2(UiLayout.Width / 2 - titleSize.X / 2, boxRect.Y + 20), Color.MediumPurple);
        spriteBatch.DrawString(font, desc, new Vector2(UiLayout.Width / 2 - descSize.X / 2, boxRect.Y + 60), Color.White * 0.9f, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);

        // Highlight relevant button
        string targetBtn = CurrentStep switch
        {
            TutorialStep.Focus => TextService.Instance.Get("BTN_FOCUS"),
            TutorialStep.ManifestFirst => "Speck", // This is tricky as manifestation names are dynamic
            _ => ""
        };

        if (!string.IsNullOrEmpty(targetBtn))
        {
            foreach (var btn in buttons)
            {
                if (btn.Text.Contains(targetBtn, StringComparison.OrdinalIgnoreCase) && btn.IsVisible())
                {
                    spriteBatch.Draw(pixel, new Rectangle(btn.Bounds.X - 5, btn.Bounds.Y - 5, btn.Bounds.Width + 10, btn.Bounds.Height + 10), Color.Yellow * 0.3f);
                }
            }
        }
    }
}
