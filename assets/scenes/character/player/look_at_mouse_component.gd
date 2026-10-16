class_name LookAtMouseComponent2D extends Node

@export
var _look_at_mouse_nodes: Array[Node2D] = []

func _physics_process(_delta: float) -> void:
	# https://github.com/DefectiveDev/godot-wild-jam-98/issues/1
	# Make player Aim at Mouse 

	for node in _look_at_mouse_nodes:
		node.look_at(node.get_global_mouse_position())
		node.rotation_degrees += 90
