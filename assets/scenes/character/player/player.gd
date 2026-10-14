class_name Player extends DefaultCharacter2D


@onready var rotation_child_node = $Rotation

func _init():
	add_to_group("player")

func on_death() -> void:
	super.on_death()
	print("Player has died")

func _physics_process(_delta: float) -> void:
	# https://github.com/DefectiveDev/godot-wild-jam-98/issues/2
	# Add player movement
	#
	# Get the input direction and handle the movement/deceleration.
	# As good practice, you should replace UI actions with custom gameplay actions.
	var direction: Vector2 = Vector2(
		Input.get_axis("ui_left", "ui_right"),
		Input.get_axis("ui_up", "ui_down")
		).normalized()
	

	if direction.x:
		velocity.x = direction.x * current_speed
	else:
		velocity.x = move_toward(velocity.x, 0, current_speed)

	if direction.y:
		velocity.y = direction.y * current_speed
	else:
		velocity.y = move_toward(velocity.y, 0, current_speed)

	move_and_slide()


	# https://github.com/DefectiveDev/godot-wild-jam-98/issues/1
	# Make player Aim at Mouse 
	rotation_child_node.look_at(get_global_mouse_position())
	rotation_child_node.rotation_degrees += 90
