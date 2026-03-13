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

    private Point? _mockMousePos;
    private HashSet<Keys> _mockKeys = new();

    public void SetMousePosition(Point pos) => _mockMousePos = pos;
    public void MockKeyPress(Keys key) => _mockKeys.Add(key);

    public void Update()
    {
        _lastMouse = _currentMouse;
        _currentMouse = Mouse.GetState();
        _lastKey = _currentKey;
        _currentKey = Keyboard.GetState();
        // Mock keys are cleared after each update
    }

    public void ClearMock()
    {
        _mockKeys.Clear();
    }

    public bool IsKeyPressed(Keys key) 
    {
        if (_mockKeys.Contains(key)) return true;
        return _currentKey.IsKeyDown(key) && _lastKey.IsKeyUp(key);
    }

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
