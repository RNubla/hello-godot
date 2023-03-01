using Godot;
using System;

public partial class CharacterController3D : CharacterBody3D
{
    /*
        Main class of the addon, contains abilities array for character movements
        Emitted when the character controller performs a step, called at the of the 
        move
        function when a move accumulator for a step has ended
    */
    [Signal]
    public delegate void stepped();

    /* 
		Emitted when touching the ground after being airborn, called in the 
		move function.
	 */
    [Signal]
    public delegate void landed();

    /* 
		Emitted when a jump is processed, is called when [JumpAbility3D] is active.
	 */
    [Signal]
    public delegate void jumped();

    /*
		Emitted when a crouch is started, is called when [CrouchAbility3D] is active.
	*/
    [Signal]
    public delegate void crouch();

    /*
		Emitted when a crouch is finished, is called when [CrouchAbility3D] is deactive
	*/
    [Signal]
    public delegate void uncrouched();

    /*
		Emitted when a sprint started, is called when [SprintAbility3D] is active.
	*/
    [Signal]
    public delegate void sprinted();

    /*
		Emitted when a fly mode is started, called when [FlyModeAbility3D] is active.
	*/
    [Signal]
    public delegate void fly_mode_actived();

    /*
		Emitted when a fly mode is finished, called when [FlyModeAbility3D] is deactive
	*/
    [Signal]
    public delegate void fly_mode_deactived();

    /*
		Emitted when emerged in water.
		Called when the height of the water depth returned from the 
		[b]get_depth_on_water()[/b] function of [SwimAbility3D] is greater than the 
		minimum height defined in [b]submerged_height[/b].
	*/
    [Signal]
    public delegate void emerged();

    /* 
        Emitted when submerged in water.
        Called when the water depth height returned from the 
        [b]get_depth_on_water()[/b] function of [SwimAbility3D] is less than the 
        minimum height defined in [b]submerged_height[/b].
     */
    [Signal]
    public delegate void submerged();

    /*  Emitted when it starts to touch the water. */
    [Signal]
    public delegate void entered_the_water();

    /* ## Emitted when it stops touching the water. */
    [Signal]
    public delegate void exit_the_water();

    /* 
        Emitted when water starts to float.
        Called when the height of the water depth returned from the 
        [b]get_depth_on_water()[/b] function of [SwimAbility3D] is greater than the 
        minimum height defined in [b]floating_height[/b]. */
    [Signal]
    public delegate void started_floating();

    /* 
        Emitted when water stops floating.
        Called when the water depth height returned from the 
        [b]get_depth_on_water()[/b] function of [SwimAbility3D] is less than the 
        minimum height defined in [b]floating_height[/b]. */
    [Signal]
    public delegate void stopped_floating();

    [ExportGroup("Movement")]
    /* 
        Controller Gravity Multiplier
        The higher the number, the faster the controller will fall to the ground and 
        your jump will be shorter. */
    [Export] private float gravity_multiplier = 3.0f;

    /* 
    Controller base speed
    Note: this speed is used as a basis for abilities to multiply their 
    respective values, changing it will have consequences on [b]all abilities[/b]
    that use velocity. */
    [Export] private float speed = 10.0f;

    /*  Time for the character to reach full speed */
    [Export] private float acceleration = 8.0f;

    /*  Time for the character to stop walking */
    [Export] private float deceleration = 10.0f;

    /*  Sets control in the air */
    [Export] private float air_control = 0.3f;

    [ExportGroup("Sprint")]

    /* ## Speed to be multiplied when active the ability */
    [Export] public float sprint_speed_multiplier = 1.6f;


    [ExportGroup("Footsteps")]

    /* ## Maximum counter value to be computed one step */
    [Export] public float step_lengthen = 0.7f;

    /* ## Value to be added to compute a step, each frame that the character is walking this value  */
    /* ## is added to a counter */
    [Export] public float step_interval = 6.0f;


    [ExportGroup("Crouch")]

    /* ## Collider height when crouch actived */
    [Export] public float height_in_crouch = 1.0f;

    /* ## Speed multiplier when crouch is actived */
    [Export] public float crouch_speed_multiplier = 0.7f;


    [ExportGroup("Jump")]

    /* ## Jump/Impulse height */
    [Export] public float jump_height = 10f;


    [ExportGroup("Fly")]

    /* ## Speed multiplier when fly mode is actived */
    [Export] public float fly_mode_speed_modifier = 2f;


    [ExportGroup("Swim")]

    /* ## Minimum height for [CharacterController3D] to be completely submerged in water. */
    [Export] public float submerged_height = 0.36f;

    /* ## Minimum height for [CharacterController3D] to be float in water. */
    [Export] public float floating_height = 0.75f;

    /* ## Speed multiplier when floating water */
    [Export] public float on_water_speed_multiplier = 0.75f;

    /* ## Speed multiplier when submerged water */
    [Export] public float submerged_speed_multiplier = 0.5f;


    [ExportGroup("Abilities")]
    /* ## List of movement skills to be used in processing this class. */
    // [Export] var abilities_path: Array[NodePath]

    /* ## List of movement skills to be used in processing this class. */
    public Godot.Collections.Array<MovementAbility3D> _abilities;

    /* ## Result direction of inputs sent to [b]move()[/b]. */
    private Vector3 _direction = new Vector3();

    /* ## Current counter used to calculate next step. */
    private float _step_cycle = 0;

    /* ## Maximum value for _step_cycle to compute a step. */
    private float _next_step = 0;

    /* ## Character controller horizontal speed. */
    private Vector3 _horizontal_velocity;

    /* ## Base transform node to direct player movement */
    /* ## Used to differentiate fly mode/swim moves from regular character movement. */
    private Node3D _direction_base_node;

    /* ## Get the gravity from the project settings to be synced with RigidDynamicBody nodes. */
    @onready var gravity: float = (ProjectSettings.get_setting("physics/3d/default_gravity") * gravity_multiplier)

/* ## Collision of character controller. */
@onready var collision: CollisionShape3D = get_node(NodePath("Collision"))

/* ## Above head collision checker, used for crouching and jumping. */
@onready var head_check: RayCast3D = get_node(NodePath("Head Check"))

/* ## Basic movement ability. */
@onready var walk_ability: WalkAbility3D = get_node(NodePath("Walk Ability 3D"))

/* ## Crouch Ability, change size collider and velocity. */
@onready var crouch_ability: CrouchAbility3D = get_node(NodePath("Crouch Ability 3D"))

/* ## Ability that adds extra speed when actived. */
@onready var sprint_ability: SprintAbility3D = get_node(NodePath("Sprint Ability 3D"))

/* ## Simple ability that adds a vertical impulse when actived (Jump). */
@onready var jump_ability: JumpAbility3D = get_node(NodePath("Jump Ability 3D"))

/* ## Ability that gives free movement completely ignoring gravity. */
@onready var fly_ability: FlyAbility3D = get_node(NodePath("Fly Ability 3D"))

/* ## Swimming ability. */
@onready var swim_ability: SwimAbility3D = get_node(NodePath("Swim Ability 3D"))

/* ## Stores normal speed */
@onready var _normal_speed: int = speed

/* ## True if in the last frame it was on the ground */
var _last_is_on_floor = false;

/* ## Default controller height, affects collider */
var _default_height : float

}
