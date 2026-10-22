using Godot;

public partial class PauseScreen : CanvasLayer
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _TogglePause();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel"))
        {
            _TogglePause();
        }
    }

    public void _TogglePause()
    {
        GetTree().Paused = !GetTree().Paused;
        Visible = GetTree().Paused;
    }
}
