class_name AttackComponent extends Node

@export
var damage_per_second: float = 10.0

@export
var attack_cooldown_timer: Timer = null

var _attack_cooldown_reached = true

func _ready():
	attack_cooldown_timer.timeout.connect(func(): _attack_cooldown_reached = true)

func attack(attack_func: Callable, attack_args: Array) -> void:
	if _attack_cooldown_reached:
		_attack_cooldown_reached = false
		attack_func.callv(attack_args)
		attack_cooldown_timer.start()
