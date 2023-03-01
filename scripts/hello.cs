using Godot;
using System;

public partial class hello : Node
{
    private Godot.MeshInstance3D cube;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        cube = GetNode<Godot.MeshInstance3D>("Cube");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        cube.Rotate(new Vector3(1f, 1f, 1f).Normalized(), 1f);
        // var d = new Quaternion(2f, 10f, 3f, 6f);
        // cube.Transform.Rotated(new Vector3(1f, 2f, 3f), 5f);
        // cube.Duplicate(3);
    }
}
