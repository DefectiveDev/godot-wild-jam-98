using System;
using Godot;

[GlobalClass]
public partial class MovementComponent2D : Node
{
    [Export]
    public float MaxSpeed = 300;

    public float CurrentSpeed;

    public override void _Ready()
    {
        base._Ready();
        CurrentSpeed = MaxSpeed;
    }

    public void Update(double _delta, CharacterBody2D characterBody2D, Vector2 moveDirection)
    {
        Vector2 velocity = characterBody2D.Velocity;
        if (moveDirection.X != 0)
        {
            velocity.X = moveDirection.X * CurrentSpeed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(velocity.X, 0, CurrentSpeed);
        }
        if (moveDirection.Y != 0)
        {
            velocity.Y = moveDirection.Y * CurrentSpeed;
        }
        else
        {
            velocity.Y = Mathf.MoveToward(velocity.Y, 0, CurrentSpeed);
        }

        characterBody2D.Velocity = velocity;
        characterBody2D.MoveAndSlide();
    }
}
