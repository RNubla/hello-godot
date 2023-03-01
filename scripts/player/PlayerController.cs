using Godot;
using System;

public partial class PlayerController : CharacterBody3D
{
    [Export]
    public const float Speed = 350.0f;
    public const float JumpVelocity = 4.5f;
    public float camera_x_rot = 0.0f;
    private const float CAMERA_X_ROT_MIN = -89.9f;
    private const float CAMERA_X_ROT_MAX = 70f;

    [Export]
    private float _sensitivity = 0.25f;
    [Export] Camera3D mainCamera;
    private Quaternion _cameraRotation;

    [Export] Node3D headNode;
    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion eventMouseMotion)
        {
            CameraRotate(eventMouseMotion);
        }
    }


    private void CameraRotate(InputEventMouseMotion mouseMotion)
    {
        // Allows the player to look left and right
        RotateY(Mathf.DegToRad(-mouseMotion.Relative.X * _sensitivity));
        // After relative transforms, camera needs to be renormalized.
        mainCamera.Orthonormalize();
        headNode.RotateX(Mathf.DegToRad(-mouseMotion.Relative.Y * _sensitivity));
        Vector3 rotDeg = headNode.RotationDegrees;
        rotDeg.X = Mathf.Clamp(rotDeg.X, CAMERA_X_ROT_MIN, CAMERA_X_ROT_MAX);
        headNode.RotationDegrees = rotDeg;
    }


    public override void _Ready()
    {
        GD.Print(mainCamera);
        // mainCamera = GetNode<Camera3D>("Head/MainCamera");
        // GD.Print("GetChild: ", GetNode<Camera3D>("Head/MainCamera"));
        // headNode = GetNode<Node3D>("Head");
        // mainCamera = GetNode<Camera3D>("Head/MainCamera");
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
            velocity.Y -= gravity * (float)delta;

        // Handle Jump.
        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
            velocity.Y = JumpVelocity;

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 inputDir = Input.GetVector("left", "right", "forward", "backward");
        Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
        if (direction != Vector3.Zero)
        {
            velocity.X = direction.X * Speed * (float)delta;
            velocity.Z = direction.Z * Speed * (float)delta;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
        }

        Velocity = velocity;
        MoveAndSlide();
    }
}
