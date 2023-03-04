using Godot;
using System;

[Tool]
public partial class RotationDemo : Node3D
{
    [Export]
    NodePath child;
    [Export]
    NodePath parent;

    private Node3D parentObject;
    private Node3D childObject;

    [Export]
    public Vector3 parentObjectRotation;
    public override void _Ready()
    {
        //Note that in order to link nodes to code,
        //we must pass a nodepath to from the editor
        //and set it on the _Ready function
        parentObject = GetNode<Node3D>(parent);
        childObject = GetNode<Node3D>(child);
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (Engine.IsEditorHint())
        {
            parentObject.GlobalRotate(Vector3.Up, (0.5f * (float)delta));
            childObject.RotationDegrees = parentObject.RotationDegrees;
        }
    }
}
