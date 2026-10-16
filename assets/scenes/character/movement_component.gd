class_name MovementComponent2D extends Node

@export
var max_speed: float = 300.0

@onready var current_speed = max_speed

func update(_delta: float, physics_body_2d: PhysicsBody2D, move_direction: Vector2) -> void:
	if move_direction.x:
		physics_body_2d.velocity.x = move_direction.x * current_speed

	else:
		physics_body_2d.velocity.x = move_toward(physics_body_2d.velocity.x, 0, current_speed)

	if move_direction.y:
		physics_body_2d.velocity.y = move_direction.y * current_speed
	else:
		physics_body_2d.velocity.y = move_toward(physics_body_2d.velocity.y, 0, current_speed)

	physics_body_2d.move_and_slide()
