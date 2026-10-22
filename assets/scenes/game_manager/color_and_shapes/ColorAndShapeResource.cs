using System;
using Godot;

[GlobalClass]
public partial class ColorAndShapeResource : Resource
{
    [Export]
    public Color Color;

    [Export]
    public Texture2D Shape;
}
