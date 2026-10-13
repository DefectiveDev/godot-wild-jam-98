extends CharacterBody2D

class_name Player

@export
var max_health: int = 100

@onready var current_health: int = max_health

@export
var damage_per_second = 10

@export
var max_speed = 300.0

@onready var current_speed = max_speed

@onready var visual = $Visuals

func _init():
	add_to_group("player")

func _physics_process(_delta: float) -> void:
	# https://github.com/DefectiveDev/godot-wild-jam-98/issues/2
	# Add player movement
	#
	# Get the input direction and handle the movement/deceleration.
	# As good practice, you should replace UI actions with custom gameplay actions.
	var directionX := Input.get_axis("ui_left", "ui_right")
	if directionX:
		velocity.x = directionX * current_speed
	else:
		velocity.x = move_toward(velocity.x, 0, current_speed)

	var directionY := Input.get_axis("ui_up", "ui_down")
	if directionY:
		velocity.y = directionY * current_speed
	else:
		velocity.y = move_toward(velocity.y, 0, current_speed)

	move_and_slide()


	# https://github.com/DefectiveDev/godot-wild-jam-98/issues/1
	# Make player Aim at Mouse 
	visual.look_at(get_global_mouse_position())
	visual.rotation_degrees += 90
