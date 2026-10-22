using System;
using Godot;

public partial class ReloadButton : Godot.Button
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready() => ButtonDown += () => GetTree().ReloadCurrentScene();
}
