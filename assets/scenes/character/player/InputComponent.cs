using System;
using Godot;

[GlobalClass]
public partial class InputComponent : Node
{
    Vector2 _moveDirection = Vector2.Zero;

    /// <summary>
    /// This will update all variables in this component. Please call within Process or Physics_process to get continuous updates
    /// </summary>
    public void Update() =>
        _moveDirection = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

    /// <summary>
    /// call InputComponent.update() for the value of movement direction to change
    /// </summary>
    /// <returns></returns>
    public Vector2 GetMoveDirection() => _moveDirection;
}
