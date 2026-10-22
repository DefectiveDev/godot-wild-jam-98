using Godot;
using Godot.Collections;

public partial class LookAtMouseComponent2D : Node
{
    [Export]
    Array<Node2D> _lookAtMouseNodes = [];

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        foreach (Node2D node in _lookAtMouseNodes)
        {
            node.LookAt(node.GetGlobalMousePosition());
            node.RotationDegrees += 90;
        }
    }
}
