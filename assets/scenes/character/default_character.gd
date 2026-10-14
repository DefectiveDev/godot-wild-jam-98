class_name DefaultCharacter2D extends CharacterBody2D


@export
var max_health: int = 100

@onready var current_health: int = max_health:
	set(v):
		current_health = max(v, 0)

		if current_health <= 0:
			on_death()

	get:
		return current_health

@export
var damage_per_second: float = 10.0

@export
var max_speed: float = 300.0

@onready var current_speed = max_speed

@warning_ignore("UNUSED_SIGNAL")
signal hit(attack_func: Callable)

signal death()

func on_death() -> void:
	death.emit()

func attack(_target: DefaultCharacter2D):
	pass
