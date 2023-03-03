@tool
extends SkeletonIK3D

@export var playing: bool = false


func _process(delta):
	if not Engine.is_editor_hint():
		return
		
	if self.playing and not self.is_running():
		self.start()

	if not self.playing and self.is_running():
		self.stop()
