extends CharacterBody2D


const SPEED = 300.0

var player: Player = null

func _init():
	max_slides = 1

func _physics_process(_delta: float) -> void:
	# https://github.com/DefectiveDev/godot-wild-jam-98/issues/4
	# Have the enemy follow the player

	if not player:
		player = GameManagerSingleton.GAME_MANAGER.player
		print(GameManagerSingleton.GAME_MANAGER.player)
	var direction := position.direction_to(player.position)
	if direction:
		velocity = direction * SPEED
	else:
		velocity.x = move_toward(velocity.x, 0, SPEED)
		velocity.y = move_toward(velocity.y, 0, SPEED)

	move_and_slide()
