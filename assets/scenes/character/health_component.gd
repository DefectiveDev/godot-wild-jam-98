class_name HealthComponent extends Node

@export
var max_health: int = 100

@onready var _current_health: int = max_health


var _is_dead := false

func set_health(health: int) -> int:
	if _is_dead:
		return _current_health

	if health != get_health():
		health_changed.emit(self)

	_current_health = clampi(health, 0, max_health)

	if get_health() <= 0:
		_is_dead = true
		died.emit()

	return _current_health

func get_health() -> int:
	return _current_health

func get_is_dead() -> bool:
	return _is_dead

signal health_changed(health_component: HealthComponent)

signal died()
