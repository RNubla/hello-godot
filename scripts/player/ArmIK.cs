using Godot;
using System;
// using System.Collections.Generic;
using System.Linq;

public partial class ArmIK : Node
{
    // The chain of bones that make up the arm
    [Export] private Godot.Collections.Array<Skeleton3D> _bones = new Godot.Collections.Array<Skeleton3D>();
    // The target position and orientation of the end effector
    private Vector3 _targetPosition = Vector3.Zero;
    private Quaternion _targetOrientation = Quaternion.Identity;


    // Tolerance for when the IK solution is considered valid
    private const float Tolerance = 0.1f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        // Get the current position and orientation of the end effector
        GD.Print("bones: ", _bones[0].FindBone("root"));
        // var endEffectorPosition = _bones[_bones.Count()-1].GlobalTransform.origin;
    }
}
