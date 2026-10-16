class_name SpawnComponent extends Node

@export
var spawn_timer: Timer

@export
var scene_to_spawn: PackedScene

func _ready() -> void:
	spawn_timer.timeout.connect(on_spawn_timer_timeout)

func on_spawn_timer_timeout():
	pass
