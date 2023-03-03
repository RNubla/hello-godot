using Godot;
using System;

[Tool]
public partial class IkPreview : SkeletonIK3D
{
    [Export]
    public Boolean playing = false;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        // if (!Engine.IsEditorHint())
        // {
        //     return;
        // }
        if (this.playing && !this.IsRunning())
        {
            this.Start();
        }
        if (!this.playing && this.IsRunning())
        {
            this.Stop();
        }
    }
}
