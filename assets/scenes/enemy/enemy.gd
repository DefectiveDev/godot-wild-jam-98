extends CharacterBody2D


@export
var max_health: int = 100

@onready var current_health: int = max_health

@export
var damage_per_second = 10

@export
var max_speed = 300.0

@onready var current_speed = max_speed

var player: Player = null

func _init():
	max_slides = 1

func _physics_process(_delta: float) -> void:
	# https://github.com/DefectiveDev/godot-wild-jam-98/issues/4
	# Have the enemy follow the player

	if not player:
		player = GameManagerSingleton.GAME_MANAGER.player
	var direction := position.direction_to(player.position)
	if direction:
		velocity = direction * current_speed
	else:
		velocity.x = move_toward(velocity.x, 0, current_speed)
		velocity.y = move_toward(velocity.y, 0, current_speed)
	
	var collision = move_and_collide(velocity, true)

	if collision:
		print(collision.get_collider())
	move_and_slide()
