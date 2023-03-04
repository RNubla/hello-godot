using Godot;
using System;

[Tool]
public partial class RotationDemo : Node3D
{
    [Export]
    private Node3D childObject = new Node3D();
    [Export]
    private NodePath childObjectNodePath;

    [Export]
    public Vector3 parentObjectRotation;
    public override void _Ready()
    {
        // childObject = GetNode<Node3D>(".");
        childObject = GetNode<Node3D>(childObjectNodePath);
        // GD.Print("_Ready: ", childObject);
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (Engine.IsEditorHint())
        {
            // GD.Print(childObjectNodePath);
            this.RotateX(0.5f * (float)delta);
            // childObject.RotateX(1);
            // this.childObject.RotateX(0.5f * (float)delta);
            this.parentObjectRotation.X = this.Rotation.X;
            // childObject.Transform.Rotated(Vector3.Left, 0.5f * (float)delta);
            // this.childObject.Rotate(Vector3.Left, this.GlobalRotation.X);
        }
    }
}
