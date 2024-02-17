using Godot;

public partial class Game : Scene
{
    // public void _OnButtonPressed() => SceneManagerNode.ChangeToScene("Menu");

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

		if (@event is InputEventKey eventKey)
		{
			if (eventKey.Pressed && eventKey.Keycode == Key.Escape)
				SceneManagerNode.Quit();
		}
    }
}
