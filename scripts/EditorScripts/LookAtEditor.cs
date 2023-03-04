using Godot;
using System;

[Tool]
public partial class LookAtEditor : Node3D
{
    [Export]
    public Node3D lookAtObject;

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (!Engine.IsEditorHint())
        {
            return;
        }
        LookAt(lookAtObject.GlobalPosition);

    }
}
