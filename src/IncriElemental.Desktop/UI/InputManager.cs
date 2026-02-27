using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace IncriElemental.Desktop.UI;

public class InputManager
{
    private MouseState _currentMouse;
    private MouseState _lastMouse;
    private KeyboardState _currentKey;
    private KeyboardState _lastKey;

    public Point MousePosition => _currentMouse.Position;

    public void Update()
    {
        _lastMouse = _currentMouse;
        _currentMouse = Mouse.GetState();
        _lastKey = _currentKey;
        _currentKey = Keyboard.GetState();
    }

    public bool IsLeftClick() => _currentMouse.LeftButton == ButtonState.Pressed && _lastMouse.LeftButton == ButtonState.Released;

    public bool IsKeyPressed(Keys key) => _currentKey.IsKeyDown(key) && _lastKey.IsKeyUp(key);
}
