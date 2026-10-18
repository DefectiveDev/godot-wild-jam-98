class_name AttackComponent extends Node

@export
var damage_per_second: float = 10.0

@export
var attack_cooldown_timer: Timer = null

var _attack_cooldown_reached = true

func _ready():
	attack_cooldown_timer.timeout.connect(func(): _attack_cooldown_reached = true)

## Call an attack with respect to a cool down timer. To add arguments use Callable.bind()
func attack_on_cooldown(attack_func: Callable) -> void:
	if _attack_cooldown_reached:
		_attack_cooldown_reached = false
		attack_func.call()
		attack_cooldown_timer.start()
