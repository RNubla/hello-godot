using Godot;
using System;

public partial class Bullet : Area3D
{
    [Export] private float muzzleVelocity = 25f;
    private Vector3 gravity = new Vector3(0, ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle(), 0);
    private Vector3 velocity = Vector3.Zero;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        velocity -= gravity * (float)delta;
        LookAt(Transform.Origin + -velocity.Normalized(), Vector3.Up);
        // Transform = new Transform(Transform.Basis, Transform.Origin + velocity * delta);
        Transform = new Transform3D(Transform.Basis, Transform.Origin + velocity * (float)delta);

    }
}
