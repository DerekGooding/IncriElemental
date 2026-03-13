using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using IncriElemental.Core.Engine;

namespace IncriElemental.Desktop.UI;

public class InputManager
{
    private MouseState _currentMouse;
    private MouseState _lastMouse;
    private KeyboardState _currentKey;
    private KeyboardState _lastKey;

    public float UiScale { get; set; } = 1.0f;
    public bool IsTooltipPinned { get; private set; } = false;

    public Point MousePosition => new Point((int)(_currentMouse.X / UiScale), (int)(_currentMouse.Y / UiScale));
    public int ScrollDelta => _currentMouse.ScrollWheelValue - _lastMouse.ScrollWheelValue;

    public void Update()
    {
        _lastMouse = _currentMouse;
        _currentMouse = Mouse.GetState();
        _lastKey = _currentKey;
        _currentKey = Keyboard.GetState();
    }

    public bool IsLeftClick() => _currentMouse.LeftButton == ButtonState.Pressed && _lastMouse.LeftButton == ButtonState.Released;
    public bool IsRightClick() => _currentMouse.RightButton == ButtonState.Pressed && _lastMouse.RightButton == ButtonState.Released;

    public bool IsKeyPressed(Keys key) => _currentKey.IsKeyDown(key) && _lastKey.IsKeyUp(key);

    public void HandleHotkeys(GameEngine engine, Action<GameTab> setTab)
    {
        // Space -> Focus
        if (IsKeyPressed(Keys.Space))
        {
            engine.Focus();
        }

        // 1-5 -> Tabs
        if (IsKeyPressed(Keys.D1)) setTab(GameTab.Void);
        if (IsKeyPressed(Keys.D2)) setTab(GameTab.Spire);
        if (IsKeyPressed(Keys.D3)) setTab(GameTab.World);
        if (IsKeyPressed(Keys.D4)) setTab(GameTab.Constellation);
        if (IsKeyPressed(Keys.D5)) setTab(GameTab.Flow);

        // UI Scaling: [ and ]
        if (IsKeyPressed(Keys.OemOpenBrackets)) UiScale = Math.Max(0.5f, UiScale - 0.1f);
        if (IsKeyPressed(Keys.OemCloseBrackets)) UiScale = Math.Min(2.0f, UiScale + 0.1f);

        // Tooltip Pinning: P or Right Click
        if (IsKeyPressed(Keys.P) || IsRightClick())
        {
            IsTooltipPinned = !IsTooltipPinned;
        }
    }

    public void ProcessButtons(List<Button> buttons, GameTab currentTab, float scrollOffset)
    {
        var isClick = IsLeftClick();
        foreach (var btn in buttons)
        {
            if (!btn.IsVisible()) continue;
            var offset = btn.Tab == GameTab.None ? 0 : (int)scrollOffset;
            if (btn.Tab == currentTab || btn.Tab == GameTab.None)
            {
                btn.Update(MousePosition, offset);
                if (isClick) btn.CheckClick(MousePosition, offset);
            }
        }
    }
}
