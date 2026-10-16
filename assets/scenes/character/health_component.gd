class_name HealthComponent extends Node

@export
var max_health: int = 100

@onready var _current_health: int = max_health

func set_health(health: int) -> int:
	_current_health = clampi(health, 0, max_health)

	if health != get_health():
		health_changed.emit()
	
	if get_health() <= 0:
		_on_death()

	return _current_health

func get_health() -> int:
	return _current_health

signal health_changed(health_component: HealthComponent)

signal died()

func _on_death() -> void:
	died.emit()
