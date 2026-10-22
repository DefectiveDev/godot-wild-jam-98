using Godot;
using Godot.Collections;

[GlobalClass]
public partial class FlashBeamArea2D : Area2D
{
    [Signal]
    public delegate void HasOverlappingBodiesProcessEventHandler(Array<Node2D> overlappingBoddies);

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        Array<Node2D> overlappingBoddies = GetOverlappingBodies();
        if (overlappingBoddies.Length() > 0)
        {
            EmitSignalHasOverlappingBodiesProcess(overlappingBoddies);
        }
    }
}
