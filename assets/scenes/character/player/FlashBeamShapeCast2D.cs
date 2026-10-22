using Godot;
using Godot.Collections;

[GlobalClass]
public partial class FlashBeamShapeCast2D : ShapeCast2D
{
    [Signal]
    public delegate void CollisionHappenedEventHandler(Array<Node2D> collisions);

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (IsColliding())
        {
            Array<Node2D> collisions = [];
            for (int i = 0; i < GetCollisionCount(); i++)
            {
                if (GetCollider(i) is Node2D node2D)
                {
                    collisions.Add(node2D);
                }
            }
            EmitSignalCollisionHappened(collisions);
        }
    }
}
