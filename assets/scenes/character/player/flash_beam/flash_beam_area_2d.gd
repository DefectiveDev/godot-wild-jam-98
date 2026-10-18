extends Area2D


signal has_overlapping_bodies_process(overlapping_boddies: Array[Node2D])

func _physics_process(_delta: float) -> void:
	var overlapping_bodies = get_overlapping_bodies()
	if not overlapping_bodies.is_empty():
		has_overlapping_bodies_process.emit(overlapping_bodies)

