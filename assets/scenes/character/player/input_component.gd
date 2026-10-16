class_name InputComponent extends Node

var _move_direction: Vector2 = Vector2.ZERO

## This will update all variables in this component. Please call within _process or _physics_process to get continuous updates
func update():
	_move_direction = Input.get_vector("ui_left", "ui_right", "ui_up", "ui_down")

## call InputComponent.update() for the value of movement direction to change
func get_move_direction() -> Vector2:
	return _move_direction
