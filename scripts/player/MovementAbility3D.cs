using Godot;
using System;

public partial class MovementAbility3D : Node3D
{
    /* ## Movement skill abstract class.
    ## 
    ## It contains a flag to enable/disable the movement skill, signals emitted when this flag was modified. */

    [Export] public Boolean _active = false;
    private Boolean _last_active = false;

    /* ## Emitted when ability has been active, is called when [b]set_active()[/b] is set to true */
    [Signal] public delegate Boolean Actived();

    /* ## Emitted when ability has been deactive, is called when [b]set_active()[/b] is set to false */
    [Signal] public delegate Boolean Deactived();

    /*
	## Returns a speed modifier, 
    ## useful for abilities that when active can change the overall speed of the [CharacterController3D], for example the [SprintAbility3D]. */
    public float GetSpeedModifier()
    {
        return 1.0f;
    }
    /* ## Returns true if ability is active */
    public Boolean IsActive()
    {
        return _active;
    }

    /* ## Defines whether or not to activate the ability */
    public void SetActive(Boolean a)
    {
        _last_active = _active;
        _active = a;
        if (_last_active != _active)
        {
            if (_active)
            {
                EmitSignal("active");
            }
            else
            {
                EmitSignal("deactived");
            }
        }
    }
    /* ## Change current velocity of [CharacterController3D].
    ## In this function abilities can change the way the character controller behaves based on speeds and other parameters received. */
    public Vector3 Apply(Vector3 velocity, float speed, Boolean isOnFloor, Vector3 direction, float delta)
    {
        return velocity;
    }
}
