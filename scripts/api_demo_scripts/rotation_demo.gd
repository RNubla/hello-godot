@tool
extends Node3D

@export  var parent : NodePath
@export  var child : NodePath

var parentObject : Node3D
var childObject : Node3D
# Called when the node enters the scene tree for the first time.
func _ready():
	parentObject = get_node(parent)
	childObject = get_node(child)



# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
#	print(childObject)
	if Engine.is_editor_hint():
#		print(parentObject)
#		pass
		parentObject.rotate_x(0.5 * delta)
		childObject.rotation_degrees = parentObject.rotation_degrees
#		childObject.rotate_x(parentObject.rotation.x)
#		print(childObject2)
		
